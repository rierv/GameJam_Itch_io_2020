using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogeManager : MonoBehaviour
{

 
    public GameObject DialogueBox;
    private GameObject boxIstance;



    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        

      
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DisplayNextSentence();
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
            boxIstance.GetComponent<Transform>().GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
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
        Time.timeScale = 1;
        Debug.Log("End");    
    }
}
