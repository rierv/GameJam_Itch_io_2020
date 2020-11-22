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


        Time.timeScale = 0;
        boxIstance = Instantiate(DialogueBox, new Vector2(0, 0), Quaternion.identity);
        
        Debug.Log("Starting con  with " + dialogue.NPCname);

        boxIstance.GetComponent<Transform>().GetChild(0).GetChild(4).GetComponent<Text>().text = dialogue.NPCname;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
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


    }



    void EndDialogue()
    {
        Destroy(boxIstance);
        //sentences.Enqueue("CAZZO FAI ANCORA QUI");
        //sentences.Clear();
     
        NPCBlue.GetComponent<AttivatoreBotole>().EnableCunicolo();
        Time.timeScale = 1;
        Debug.Log("End");    
    }
}
