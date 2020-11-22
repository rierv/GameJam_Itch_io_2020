using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Generator : MonoBehaviour, I_Interactable
{
    public bool broken = false;
    float timer = 0;

    public GameObject linkedLight;
    
    public void PlayerEnterRange()
    {
        GlobalGameManager.instance.SelectedInteractableObj = gameObject;
    }

    public void PlayerExitRange()
    {
        GlobalGameManager.instance.SelectedInteractableObj = null;
    }

    public void Interact()
    {
        if (!broken)
        {
            broken = true;
            GetComponent<Animator>().SetTrigger("break");
            if (linkedLight != null)
            {
                linkedLight.GetComponentInChildren<Light2D>().enabled = false;
            }
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                broken = false;
                timer = 0;
                GetComponent<Animator>().SetTrigger("repair");
                if (linkedLight != null)
                {
                    linkedLight.GetComponentInChildren<Light2D>().enabled = true;
                }
            }
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        timer = 0;
    }
}
