using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FSM_Enemy : MonoBehaviour
{
    public bool playerVisible = true;
    [Range(0f, 20f)] public float range = 5f;
    public float reactionTime = 3f;
    public GameObject aimHelper;
    public GameObject target;
    public float switchTime = .5f;

    private FSM fsm;
    private float ringStart;
    private Color initialColor;
    public List<Vector2> listOfSpots;
    int curr = 0;
    GameObject heart;
    public GameObject Generator;
    GlobalGameManager GameManager;
    public float seekSpeed=2;
    float stadardSpeed;
    public float EnemyLengthofSight = 8;
    public float stunnTime = .7f;
    public bool exitStunnCoroutine = false;
    bool isStunned = false;

    void Awake()
    {
        
        if (target == null) target = FindObjectOfType<PlayerController>().gameObject;
        stadardSpeed = GetComponent<Pathfinding.AIPath>().maxSpeed;

        GameManager = GameObject.FindObjectOfType<GlobalGameManager>();
        // Define states and link actions when enter/exit/stay
        FSMState wander = new FSMState(); //off
        FSMState stunned = new FSMState(); //off
        FSMState seek = new FSMState(); //alarm

        wander.enterActions.Add(StartWander);
        stunned.stayActions.Add(BeStunned);
        wander.stayActions.Add(WanderAround);
        seek.enterActions.Add(Boost);
        seek.stayActions.Add(RingAlarm);

        // Define transitions
        FSMTransition startSeek = new FSMTransition(EnemiesAround);
        FSMTransition startStunned = new FSMTransition(GetHit);
        FSMTransition stopStunned = new FSMTransition(GetFree);
        FSMTransition returnWander = new FSMTransition(NoEnemiesAround);
        

        // Link states with transitions
        wander.AddTransition(startSeek, seek);
        wander.AddTransition(startStunned, stunned);
        seek.AddTransition(startStunned, stunned);
        seek.AddTransition(returnWander, wander);
        stunned.AddTransition(stopStunned, wander);

        if (Generator != null)
        {

            FSMState fixGenerator = new FSMState(); //alarm

            fixGenerator.stayActions.Add(Fix);

            FSMTransition goFix = new FSMTransition(isGeneratorBroken);
            FSMTransition endFixing = new FSMTransition(isGeneratorFixed);

            wander.AddTransition(goFix, fixGenerator);
            fixGenerator.AddTransition(startSeek, seek);
            fixGenerator.AddTransition(endFixing, wander);
        }
        // Setup a FSA at initial state
        fsm = new FSM(wander);

        // Start monitoring
        StartCoroutine(Patrol());
    }


    // Periodic update, run forever
    public IEnumerator Patrol()
    {
        while (true)
        {
            fsm.Update();
            yield return new WaitForSeconds(reactionTime);
        }
    }

    // CONDITIONS

    public bool EnemiesAround()
    {
        int layerMask = ~LayerMask.GetMask("Enemy");

        Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.red); //debug ray to see the ray
        Vector3 tmp = target.transform.position - transform.position;
        Vector2 dir = new Vector2(tmp.x, tmp.y);
        dir = dir.normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position+new Vector3(dir.x, dir.y, 0)*2, target.transform.position - transform.position, EnemyLengthofSight, layerMask);
        if (GameManager.PlayerVisible && !GameManager.IsInCunicolo && hit.collider&& (hit.collider.gameObject.layer==8|| hit.collider.gameObject.layer == 17))//(target.transform.position - transform.position).magnitude <= range)
        {
            return true;
        }
        
        return false;
    }

    public bool GetHit()
    {
        if(isStunned) GetComponent<BoxCollider2D>().enabled = false;

        return isStunned;
    }

    public bool GetFree()
    {
        if (heart == null && !exitStunnCoroutine)
        {
            exitStunnCoroutine = true;
            StartCoroutine(ExitStunnCoroutine());
        }
        
        return heart==null&&!isStunned;
    }

    IEnumerator ExitStunnCoroutine()
    {
        yield return new WaitForSeconds(stunnTime);
        GetComponent<BoxCollider2D>().enabled = true;
        isStunned = false;
        GetComponentInChildren<EnemyController>().stunned = false;
    }

    public bool isGeneratorBroken()
    {
        return Generator.GetComponent<Generator>().broken;
    }

    public bool isGeneratorFixed()
    {
        return !Generator.GetComponent<Generator>().broken;
    }

    public bool NoEnemiesAround()
    {

        return !EnemiesAround();
    }

    // ACTIONS
    public void StartWander()
    {
        exitStunnCoroutine = false;
        aimHelper.transform.parent = transform.parent;
        aimHelper.transform.position = transform.position;
        GetComponent<Pathfinding.AIPath>().maxSpeed = stadardSpeed;
        ringStart = Time.realtimeSinceStartup;
        curr = 0;
    }


    public void WanderAround()
    {
        if (listOfSpots.Count > 0 && Vector3.Distance(transform.position,aimHelper.transform.position)<1)
        {
            aimHelper.transform.position =new Vector3( listOfSpots[curr].x, listOfSpots[curr].y) +transform.parent.position;
            curr++;
            curr = curr % listOfSpots.Count;
        }
        //reachPoint(currentSpot);
    }

    public void BeStunned()
    {
        aimHelper.transform.parent = transform.parent;
        aimHelper.transform.position = transform.position;
    }

    public void Fix()
    {
        aimHelper.transform.position = Generator.transform.position;
    }

    public void Boost()
    {
        //animation
        GetComponent<Pathfinding.AIPath>().maxSpeed = stadardSpeed * seekSpeed;
        aimHelper.transform.position = target.transform.position;
        aimHelper.transform.parent = target.transform;
    }

    public void RingAlarm()
    {
        aimHelper.transform.localPosition = (target.transform.position-transform.position).normalized*2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Heart"&&collision.gameObject.GetComponent<Heart>().readyToHit&&!isStunned && !exitStunnCoroutine)
        {
            collision.gameObject.GetComponent<Heart>().readyToHit = false;
            collision.gameObject.layer = 11;

            isStunned = true;
            heart = collision.gameObject;
            fsm.Update();
        }
    }
}