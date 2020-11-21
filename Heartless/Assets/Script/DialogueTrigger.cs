using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject DManager;
    private BoxCollider2D bc;
    private DialogeManager DMscript;

    private void Start()
    {
        bc = gameObject.GetComponent<BoxCollider2D>();
        DMscript = DManager.GetComponent<DialogeManager>();
    }


   

    public void  TriggerDialogue()
    {
        DMscript.StartDialogue(dialogue);
        Debug.Log("funge");
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("fungo");
                TriggerDialogue();
            }
            //Debug.Log("dialogo");
        }
        else
        {
            Debug.Log("!!!!!!!!!!!!!!!!");
        }
        
    }
}
