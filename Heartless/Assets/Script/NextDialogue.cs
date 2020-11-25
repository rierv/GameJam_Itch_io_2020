using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDialogue : MonoBehaviour
{
    private Dialogue next;

    public void setNext(Dialogue d)
    {
        next = d;
    }

    public Dialogue getNext()
    {
        return (next);
    }
    
}
