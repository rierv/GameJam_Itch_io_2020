
using UnityEngine;

[System.Serializable]
public struct Line
{
   
    [TextArea(2, 5)]
    public string text;

}
[CreateAssetMenu(fileName ="New Dialogue", menuName ="Dialogue")]
public class Dialogue: ScriptableObject

{
    public string NPC_name;
    public Line[] sentences;
    public Dialogue nextDialogue;
    public Question nextQuestion;
    public bool nextIsQuestion;
}