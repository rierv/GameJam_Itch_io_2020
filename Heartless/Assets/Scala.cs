using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scala : MonoBehaviour, I_Interactable
{
    public Vector3 destination=Vector3.zero;
    
    public void PlayerEnterRange()
    {
        GlobalGameManager.instance.SelectedInteractableObj = gameObject;
    }

    public void PlayerExitRange()
    {
        GlobalGameManager.instance.SelectedInteractableObj = null;
    }

    public void Interact()
    {
        GameObject.FindObjectOfType<PlayerController>().startPosition = destination;
        GlobalGameManager.instance.SwitchFloor();
    }
}
