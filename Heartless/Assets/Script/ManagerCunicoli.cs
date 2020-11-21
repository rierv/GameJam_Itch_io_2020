using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class ManagerCunicoli : MonoBehaviour
{
    public static ManagerCunicoli instance;

    [SerializeField]
    private bool isInCunicolo;

    public GameObject groundTileMap, wallTileMap, groundCunicoloTileMap, wallCunicoloTileMap;

    [SerializeField]
    private GameObject selectedGrata;
    public GameObject SelectedGrata
    {
        get => selectedGrata;
        set => selectedGrata = value;
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            //THIS OBJECT EXISTS ALREADY
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            isInCunicolo = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (selectedGrata)
            {
                if (isInCunicolo)
                {
                    ExitCunicolo();
                }
                else
                {
                    EnterCunicolo();
                }
            }
        }
    }

    private void EnterCunicolo()
    {
        isInCunicolo = true;
        GameObject.FindWithTag("Player").GetComponent<Animator>().SetTrigger("enterBotola");
        groundCunicoloTileMap.SetActive(true);
        wallCunicoloTileMap.SetActive(true);
        wallTileMap.GetComponent<TilemapCollider2D>().enabled = false;
    }

    private void ExitCunicolo()
    {
        isInCunicolo = false;
        GameObject.FindWithTag("Player").GetComponent<Animator>().SetTrigger("enterBotola");
        groundCunicoloTileMap.SetActive(false);
        wallCunicoloTileMap.SetActive(false);
        wallTileMap.GetComponent<TilemapCollider2D>().enabled = true;
    }
}
