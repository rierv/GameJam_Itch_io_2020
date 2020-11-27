using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucozzo : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.L))
        {
            BreakBucozzo();
        }
        */
    }

    public void BreakBucozzo()
    {
        Animator[] allAnim = GetComponentsInChildren<Animator>();
        foreach (var anim in allAnim)
        {
            anim.SetTrigger("breakTile");
        }
    }
}
