using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Grata : MonoBehaviour, I_Interactable
{
    public void PlayerEnterRange()
    {
        GlobalGameManager.instance.SelectedInteractableObj = gameObject;
        GetComponent<Animator>().SetTrigger("OpenMouth");
    }

    public void PlayerExitRange()
    {
        GlobalGameManager.instance.SelectedInteractableObj = null;
        GetComponent<Animator>().SetTrigger("CloseMouth");
    }

    public void Interact()
    {
        GlobalGameManager.instance.InteractBotola();
    }
}
