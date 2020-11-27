using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Generator : MonoBehaviour, I_Interactable
{
    public bool broken = false;
    float timer = 0;
    public float breakTime = 2;
    public GameObject linkedLight;
    bool watching = false;
    private void Update()
    {
        if (broken&& watching &&linkedLight != null)

        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, linkedLight.transform.position, 2);
            if(Vector3.Distance(Camera.main.transform.position, linkedLight.transform.position) < 1)
            {
                watching = false;
                GetComponent<Animator>().SetTrigger("break");
                linkedLight.GetComponentInChildren<Light2D>().enabled = false;
                StartCoroutine(timeDelay());
            }
        }
    }

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
            Time.timeScale = 0.03f;
            watching = true;
            broken = true;
            
        }
    }

    IEnumerator timeDelay()
    {
        yield return new WaitForSeconds(0.03f);
        Time.timeScale = 1f;
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