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
    public GameObject ButtonPrefab;

    public GameObject tutorial;

    private string currentNPC;

    private Dialogue currentDialogue;

    private bool nextButton = true;



    private Queue<string> sentences;

    bool ready = false;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();



    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (ready) DisplayNextSentence();
            else
            {

                ready = true;
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentNPC = dialogue.NPC_name;

        Time.timeScale = 0;
        dialogueBoxIstance = Instantiate(DialogueBox, new Vector2(0, 0), Quaternion.identity);

        Debug.Log("Starting con  with " + dialogue.NPC_name);

        dialogueBoxIstance.GetComponent<Transform>().GetChild(0).GetChild(4).GetComponent<Text>().text = dialogue.NPC_name;

        sentences.Clear();

        foreach (Line sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence.text);
        }

        DisplayNextSentence();


    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogueBoxIstance.GetComponent<Transform>().GetChild(0).GetChild(3).GetComponent<Text>().text = sentence;
        Debug.Log(sentence);
    }

    public void StartQuestion(Question question)
    {
        Time.timeScale = 0;
        currentDialogue = null;
        questionBoxIstance = Instantiate(QuestionBox, new Vector2(0, 0), Quaternion.identity);
        questionBoxIstance.GetComponent<Transform>().GetChild(0).GetChild(2).GetComponent<Text>().text = question.questionText;
        SetChoices(question);
    }

    private void SetChoices(Question question)
    {

        Transform buttonContainer = questionBoxIstance.GetComponent<Transform>().GetChild(0).GetChild(3);
        int index = 0;
        foreach (Choice choice in question.choicesList)
        {
            GameObject newButton = Instantiate(ButtonPrefab, new Vector2(0, 0), Quaternion.identity);
            newButton.transform.SetParent(buttonContainer);
            newButton.GetComponentInChildren<Text>().text = choice.choice;


            newButton.GetComponent<Button>().onClick.AddListener(() => this.EndQuestion(choice.nextDialogue));
            newButton.GetComponent<Button>().onClick.AddListener(Click);
            if (choice.activateQuest)
            {
                switch (question.NPC_name)
                {
                    case "Yknip":
                        newButton.GetComponent<Button>().onClick.AddListener(RosaEvent);
                        break;
                    case "Eulb":
                        newButton.GetComponent<Button>().onClick.AddListener(RosaEvent);
                        break;
                    case "Neerg":
                        newButton.GetComponent<Button>().onClick.AddListener(RosaEvent);
                        break;
                }
            }
        }
    }
    void Click()
    {
        Debug.Log("click");
    }
    void RosaEvent()
    {
        tutorial.SetActive(true);
    }

    void EndQuestion(Dialogue d)
    {
        Destroy(questionBoxIstance);

        Dialogue next = d;
        Debug.Log(next.sentences[0].text);

        switch (currentNPC)
        {
            case "Yknip":
                NPCRosa.GetComponent<DialogueTrigger>().setNextDialogue(next);
                break;
            case "Eulb":
                NPCBlue.GetComponent<DialogueTrigger>().setNextDialogue(next);
                break;
            case "Neerg":
                Debug.Log("in sto if");
                NPCVerde.GetComponent<DialogueTrigger>().setNextDialogue(next);
                break;
        }
        Time.timeScale = 1;
    }



    void EndDialogue()
    {
        Destroy(dialogueBoxIstance);

        if (currentDialogue.nextIsQuestion)
        {
            StartQuestion(currentDialogue.nextQuestion);

        }
        else
        {
            Dialogue next = currentDialogue.nextDialogue;

            switch (currentNPC)
            {
                case "Yknip":
                    NPCRosa.GetComponent<DialogueTrigger>().setNextDialogue(next);
                    break;
                case "Eulb":
                    NPCBlue.GetComponent<DialogueTrigger>().setNextDialogue(next);
                    break;
                case "Neerg":
                    NPCVerde.GetComponent<DialogueTrigger>().setNextDialogue(next);
                    break;
            }
        }

        Time.timeScale = 1;
    }
}
