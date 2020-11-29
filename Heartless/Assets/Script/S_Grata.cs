﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Grata : MonoBehaviour, I_Interactable
{
    public AudioClip audio;

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
        GetComponent<AudioSource>().PlayOneShot(audio);
        //Debug.Log(GetComponent<AudioSource>().isPlaying);
        //Debug.Log(audio);
        GlobalGameManager.instance.WasInCunicolo = GlobalGameManager.instance.IsInCunicolo;
        GlobalGameManager.instance.IsInCunicolo = true;
        GlobalGameManager.instance.Player.GetComponent<PlayerController>().enabled = false;
        GlobalGameManager.instance.Player.GetComponent<Animator>().SetTrigger("enterBotola");
        //STARTING THIS ANIMATION WILL TRIGGER InteractBotola()    
        //GlobalGameManager.instance.InteractBotola();
    }
}
