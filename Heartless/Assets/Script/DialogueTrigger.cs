using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private Dialogue dialogue;
    private Question question;
    public GameObject DManager;
    private BoxCollider2D bc;
    private DialogeManager DMscript;
    private bool canTalk;
    public bool isTalking;
    public bool stopFuckingTalking = false;
    public AudioClip audioDialogue;
    public AudioClip audioQuestion;

    private void Start()
    {
        bc = gameObject.GetComponent<BoxCollider2D>();
        DMscript = DManager.GetComponent<DialogeManager>();
        canTalk = false;
        isTalking = false;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && canTalk && !isTalking && !stopFuckingTalking)
        {
            TriggerDialogue();
        }
        else if (stopFuckingTalking) stopFuckingTalking = false;
    }


   

    public void  TriggerDialogue()
    {

        isTalking = true;

        if (dialogue)
        {
            GlobalGameManager.instance.GetComponent<AudioSource>().PlayOneShot(audioDialogue);
            DMscript.StartDialogue(dialogue);
        }
        else if (question)
        {
            DMscript.StartQuestion(question);
            GlobalGameManager.instance.GetComponent<AudioSource>().PlayOneShot(audioQuestion);
        }


        }

        private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            canTalk = true;
        }
    }
  
    

    private void OnTriggerExit2D(Collider2D other)
    {
        canTalk = false;
        isTalking = false;

    }

    public void setNextDialogue(Dialogue d)
    {
        dialogue = d;
        question = null;
    }
    public void setNextQuestion(Question d)
    {
        question = d;
        dialogue = null;
    }

}
