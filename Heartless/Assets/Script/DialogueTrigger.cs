using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject DManager;
    private BoxCollider2D bc;
    private DialogeManager DMscript;
    private bool canTalk;
    private bool isTalking;

    private void Start()
    {
        bc = gameObject.GetComponent<BoxCollider2D>();
        DMscript = DManager.GetComponent<DialogeManager>();
        canTalk = false;
        isTalking = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && canTalk)
        {
            TriggerDialogue();
        }
    }


   

    public void  TriggerDialogue()
    {
        if (!isTalking)
        {
            DMscript.StartDialogue(dialogue);
            isTalking = true;
        }
     
    }
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
           
            canTalk = true;
        }
        else
        {
            Debug.Log("!!!!!!!!!!!!!!!!");
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canTalk = false;
        isTalking = false;

    }

}
