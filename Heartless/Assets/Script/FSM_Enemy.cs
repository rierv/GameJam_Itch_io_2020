using UnityEngine;
using System.Collections;

public class FSM_Enemy : MonoBehaviour
{

    [Range(0f, 20f)] public float range = 5f;
    public float reactionTime = 3f;
    public string targetTag = "Player";
    public Light ambientLight = null;
    public Color color1 = Color.red;
    public Color color2 = Color.yellow;
    public float switchTime = .5f;

    private FSM fsm;
    private float ringStart;
    private Color initialColor;

    void Start()
    {
        if (!ambientLight) return;  // Sanity

        // Define states and link actions when enter/exit/stay

        FSMState wander = new FSMState(); //off

        FSMState seek = new FSMState(); //alarm

        seek.enterActions.Add(StartAlarm);
        seek.stayActions.Add(RingAlarm);
        seek.exitActions.Add(ShutAlarm);

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
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(targetTag))
        {
            if ((go.transform.position - transform.position).magnitude <= range) return true;
        }
        return false;
    }

    public bool NoEnemiesAround()
    {
        return !EnemiesAround();
    }

    // ACTIONS

    public void StartAlarm()
    {
        initialColor = ambientLight.color;
        ringStart = Time.realtimeSinceStartup;
    }

    public void ShutAlarm()
    {
        ambientLight.color = initialColor;
    }

    public void RingAlarm()
    {
        if ((int)Mathf.Floor((Time.realtimeSinceStartup - ringStart) / switchTime) % 2 == 0)
        {
            ambientLight.color = color1;
        }
        else
        {
            ambientLight.color = color2;
        }
    }

}