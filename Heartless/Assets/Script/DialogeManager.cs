using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogeManager : MonoBehaviour
{

 
    public GameObject DialogueBox;
    private GameObject boxIstance;
    public GameObject NPCBlue;
    public GameObject NPCVerde;
    public GameObject NPCRosa;

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

    public void StartDialogue (Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentNPC = dialogue.NPC_name;

        Time.timeScale = 0;
        boxIstance = Instantiate(DialogueBox, new Vector2(0, 0), Quaternion.identity);
        
        Debug.Log("Starting con  with " + dialogue.NPC_name);

        boxIstance.GetComponent<Transform>().GetChild(0).GetChild(4).GetComponent<Text>().text = dialogue.NPC_name;

        sentences.Clear();

        foreach(Line sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence.text);
        }

        DisplayNextSentence();

        
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {

            EndDialogue();
            return;
        } 
        

            string sentence = sentences.Dequeue();
            boxIstance.GetComponent<Transform>().GetChild(0).GetChild(3).GetComponent<Text>().text = sentence;
        Debug.Log(sentence);


    }



    void EndDialogue()
    {
        Destroy(boxIstance);

        if (!currentDialogue.nextDialogue.Equals(null))
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
                    Debug.Log("in sto if");
                    NPCVerde.GetComponent<DialogueTrigger>().setNextDialogue(next);
                    break;


            }
        } else if (!currentDialogue.nextQuestion.Equals(null))
        {

        }
        else
        {
            Debug.LogError("errore");
        }

        //sentences.Enqueue("CAZZO FAI ANCORA QUI");
        //sentences.Clear();
        if (currentNPC == "Yknip")
        {
            tutorial.SetActive(true);
        } else if (currentNPC == "Eulb")
        {
            NPCBlue.GetComponent<AttivatoreBotole>().EnableCunicolo();
        }
        else if (currentNPC == "Neerg")
        {
      
        }


        Time.timeScale = 1;
        Debug.Log("End");    
    }
}
