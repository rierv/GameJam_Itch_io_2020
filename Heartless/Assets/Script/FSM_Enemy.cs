using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FSM_Enemy : MonoBehaviour
{

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

    void Start()
    {
        // Define states and link actions when enter/exit/stay
        FSMState wander = new FSMState(); //off

        FSMState seek = new FSMState(); //alarm
        wander.enterActions.Add(StartWander);
        wander.stayActions.Add(WanderAround);
        seek.stayActions.Add(RingAlarm);

        // Define transitions
        FSMTransition startSeek = new FSMTransition(EnemiesAround);
        FSMTransition returnWander = new FSMTransition(NoEnemiesAround);

        // Link states with transitions
        wander.AddTransition(startSeek, seek);
        seek.AddTransition(returnWander, wander);

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
        if ((target.transform.position - transform.position).magnitude <= range) return true;
        
        return false;
    }

    public bool NoEnemiesAround()
    {
        return !EnemiesAround();
    }

    // ACTIONS
    public void StartWander()
    {
        ringStart = Time.realtimeSinceStartup;
        curr = 0;
    }

    public void WanderAround()
    {
        if ((int)Mathf.Ceil((Time.realtimeSinceStartup - ringStart) / switchTime) % 2 == 0)
        {
            aimHelper.transform.position = listOfSpots[curr];
            curr++;
            curr = curr % listOfSpots.Count;
        }
        //reachPoint(currentSpot);
    }

    

    public void RingAlarm()
    {
        aimHelper.transform.position = target.transform.position;
    }

}