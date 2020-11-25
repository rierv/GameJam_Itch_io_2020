using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private Dialogue dialogue;

    public GameObject DManager;
    private BoxCollider2D bc;
    private DialogeManager DMscript;
    private bool canTalk;
    public bool isTalking;
    

    private void Start()
    {
        bc = gameObject.GetComponent<BoxCollider2D>();
        DMscript = DManager.GetComponent<DialogeManager>();
        canTalk = false;
        isTalking = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && canTalk &&!isTalking)
        {
            TriggerDialogue();
        }
    }


   

    public void  TriggerDialogue()
    {
        if (!isTalking)
        {
            isTalking = true;
            DMscript.StartDialogue(dialogue);
            
        }
     
    }
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            //Debug.Log("!!!!!!qqqqqqqqqqq");

            canTalk = true;
        }
        else
        {
            //Debug.Log("!!!!!!!!!!!!!!!!");
        }
        
    }
  
    

    private void OnTriggerExit2D(Collider2D other)
    {
        canTalk = false;

    }

    public void setNextDialogue(Dialogue d)
    {
        dialogue = d;
    }

}
