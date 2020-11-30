using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayOutline : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponentInParent<I_Interactable>().PlayerEnterRange();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            if(gameObject.GetComponentInParent<I_Interactable>()!=null) gameObject.GetComponentInParent<I_Interactable>().PlayerExitRange();
        }
    }
}
