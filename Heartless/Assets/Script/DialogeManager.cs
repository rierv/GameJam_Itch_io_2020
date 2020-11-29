using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogeManager : MonoBehaviour
{


    public GameObject DialogueBox;
    public GameObject QuestionBox;
    private GameObject dialogueBoxIstance;
    private GameObject questionBoxIstance;
    public GameObject NPCBlue;
    public GameObject NPCVerde;
    public GameObject NPCRosa;
    public GameObject NPCRosa2;
    public GameObject ButtonPrefab;

    public GameObject tutorial;
    public GameObject buco;

    private string currentNPC;

    private Dialogue currentDialogue;




    private Queue<string> sentences;

    bool ready = false;
    public bool isInDialogue;

    // Start is called before the first frame update
    void Awake()
    {
        isInDialogue = false;
        sentences = new Queue<string>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isInDialogue) DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentNPC = dialogue.NPC_name;

        Time.timeScale = 0;
        dialogueBoxIstance = Instantiate(DialogueBox, new Vector2(0, 0), Quaternion.identity);

        dialogueBoxIstance.GetComponent<Transform>().GetChild(0).GetChild(4).GetComponent<Text>().text = dialogue.NPC_name;

        sentences.Clear();

        foreach (Line sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence.text);
        }

        isInDialogue = true;
        DisplayNextSentence();



    }

    public void DisplayNextSentence()
    {
        if (isInDialogue)
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            string sentence = sentences.Dequeue();
            dialogueBoxIstance.GetComponent<Transform>().GetChild(0).GetChild(3).GetComponent<Text>().text = sentence;
        }
    }

    public void StartQuestion(Question question)
    {
        currentDialogue = null;
        questionBoxIstance = Instantiate(QuestionBox, new Vector2(0, 0), Quaternion.identity);
        questionBoxIstance.GetComponent<Transform>().GetChild(0).GetChild(2).GetComponent<Text>().text = question.questionText;
        SetChoices(question);
    }

    private void SetChoices(Question question)
    {

        Transform buttonContainer = questionBoxIstance.GetComponent<Transform>().GetChild(0).GetChild(3);
       
        foreach (Choice choice in question.choicesList)
        {
            GameObject newButton = Instantiate(ButtonPrefab, new Vector2(0, 0), Quaternion.identity);
            newButton.SetActive(true);
            newButton.transform.SetParent(buttonContainer);
            newButton.GetComponentInChildren<Text>().text = choice.choice;


            newButton.GetComponent<Button>().onClick.AddListener(() => this.EndQuestion(choice.nextDialogue));
     
            if (choice.activateQuest)
            {
                switch (question.NPC_name)
                {
                    case "Yknip":
                        newButton.GetComponent<Button>().onClick.AddListener(RosaEvent);
                        break;
                    case "Eulb":
                        newButton.GetComponent<Button>().onClick.AddListener(BlueEvent);
                        break;
                    case "Neerg":
                        newButton.GetComponent<Button>().onClick.AddListener(GreenEvent);
                        break;
                }
            }
        }
    }
   
    void RosaEvent()
    {
        tutorial.SetActive(true);
    }
    void BlueEvent()
    {
       
        NPCBlue.GetComponent<AttivatoreBotole>().EnableCunicolo();
    }
    void GreenEvent()
    {
        buco.GetComponent<Bucozzo>().BreakBucozzo();
    }

    public void EndQuestion(Dialogue d)
    {
        Destroy(questionBoxIstance);
        isInDialogue = false;

        Dialogue next = d;
        DialogueTrigger dt = null;

        switch (currentNPC)
        {
            case "Yknip":
                dt = NPCRosa.GetComponent<DialogueTrigger>();
                break;
            case "Eulb":
                dt = NPCBlue.GetComponent<DialogueTrigger>();
                break;
            case "Neerg":
                dt = NPCVerde.GetComponent<DialogueTrigger>();
                break;
            case "Yknip - Surprised":

                dt = NPCRosa2.GetComponent<DialogueTrigger>();
                break;
        }
        dt.setNextDialogue(next);
        dt.isTalking = false;

        Time.timeScale = 1;
    }



    public void EndDialogue()
    {
        Destroy(dialogueBoxIstance);
        isInDialogue = false;

        if (currentDialogue.nextIsQuestion)
        {
            StartQuestion(currentDialogue.nextQuestion);

        }
        else
        {
            Dialogue next = currentDialogue.nextDialogue;
            DialogueTrigger dt=null;

            switch (currentNPC)
            {
                case "Yknip":
                    dt = NPCRosa.GetComponent<DialogueTrigger>();
                    break;
                case "Eulb":
                    dt = NPCBlue.GetComponent<DialogueTrigger>();
                    break;
                case "Neerg":

                    dt = NPCVerde.GetComponent<DialogueTrigger>();
                    break;
                case "Yknip - Surprised":

                    dt = NPCRosa2.GetComponent<DialogueTrigger>();
                    break;

            }
            dt.setNextDialogue(next);
            dt.isTalking = false;
            dt.stopFuckingTalking = true;
            Time.timeScale = 1;
        }

          
    }
}
