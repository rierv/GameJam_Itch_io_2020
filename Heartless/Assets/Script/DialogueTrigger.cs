using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private BoxCollider2D bc;

    private void Start()
    {
        bc = gameObject.GetComponent<BoxCollider2D>();
    }


   

    public void  TriggerDialogue()
    {
        FindObjectOfType<DialogeManager>().StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(BoxCollider2D col)
    {
        if(col.tag == "player")
        {
            Debug.Log("dialogo");
        }
        else
        {
            Debug.Log("!!!!!!!!!!!!!!!!");
        }
        
    }
}
