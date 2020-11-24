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
    bool stunned = false;
    GameObject heart;
    public GameObject Generator;
    GlobalGameManager GameManager;
    public float seekSpeed=2;
    float stadardSpeed;
    public float EnemyLengthofSight = 8;
    void Start()
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
        int layerMask = ~LayerMask.GetMask("Nemico");

        Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.red); //debug ray to see the ray
        Vector3 tmp = target.transform.position - transform.position;
        Vector2 dir = new Vector2(tmp.x, tmp.y);
        dir = dir.normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position+new Vector3(dir.x, dir.y, 0)*2, target.transform.position - transform.position, EnemyLengthofSight, layerMask);
        if (GameManager.PlayerVisible && !GameManager.IsInCunicolo && hit.collider&& hit.collider.gameObject.tag=="Player")//(target.transform.position - transform.position).magnitude <= range)
        {
            return true;
        }
        
        return false;
    }

    public bool GetHit()
    {
        return stunned;
    }

    public bool GetFree()
    {
        if (heart == null)
        {
            stunned = false;
            GetComponentInChildren<EnemyController>().stunned = false;
        }
        return heart==null;
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
        GetComponent<Pathfinding.AIPath>().maxSpeed = stadardSpeed;
        ringStart = Time.realtimeSinceStartup;
        curr = 0;
    }


    public void WanderAround()
    {
        if (Vector3.Distance(transform.position,aimHelper.transform.position)<1)
        {
            aimHelper.transform.position = listOfSpots[curr];
            curr++;
            curr = curr % listOfSpots.Count;
        }
        //reachPoint(currentSpot);
    }

    public void BeStunned()
    {
        aimHelper.transform.position = transform.position;
    }

    public void Fix()
    {
        aimHelper.transform.position = Generator.transform.position;
    }

    public void Boost()
    {
        GetComponent<Pathfinding.AIPath>().maxSpeed = stadardSpeed*seekSpeed;
        
    }

    public void RingAlarm()
    {
        aimHelper.transform.position = target.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Heart")
        {
            stunned = true;
            heart = collision.gameObject;
            fsm.Update();
        }
    }
}