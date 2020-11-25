using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Choice
{
    [SerializeField]
    [TextArea(2, 5)]
    public string choice;
    public bool activateQuest;
    public Dialogue nextDialogue;

}
[CreateAssetMenu(fileName = "New Question", menuName = "Question")]
public class Question : ScriptableObject
{
    public string NPC_name;
    public string questionText;
    [SerializeField]
    public Choice[] choicesList;
}
