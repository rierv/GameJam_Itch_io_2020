using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucozzo : MonoBehaviour
{
    // Update is called once per frame
    void OnEnable()
    {
        GetComponent<BoxCollider2D>().enabled = GlobalGameManager.instance.bucozzoRotto;
        if (GlobalGameManager.instance.bucozzoRotto)
        {
            Animator[] allAnim = GetComponentsInChildren<Animator>();
            foreach (var anim in allAnim)
            {
                anim.SetTrigger("breakTileInstant");
            }
        }
    }

    public void BreakBucozzo()
    {
        Animator[] allAnim = GetComponentsInChildren<Animator>();
        foreach (var anim in allAnim)
        {
            anim.SetTrigger("breakTile");
        }

        GlobalGameManager.instance.bucozzoRotto = true;
        //ATTIVA COLLIDER
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GlobalGameManager.instance.SwitchFloor();
        }
    }
}
