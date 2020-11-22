using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scala : MonoBehaviour, I_Interactable
{
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
        GlobalGameManager.instance.SwitchFloor();
    }
}
