using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scala : MonoBehaviour, I_Interactable
{
    public Vector3 destination=Vector3.zero;
    public AudioClip audio;
    public int nextFloor;

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
        GlobalGameManager.instance.GetComponent<AudioSource>().PlayOneShot(audio);
        GameObject.FindObjectOfType<PlayerController>().startPosition = destination;
        GlobalGameManager.instance.SwitchFloor(nextFloor);
    }
}
