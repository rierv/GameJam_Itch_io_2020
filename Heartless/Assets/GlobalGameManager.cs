using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GlobalGameManager : MonoBehaviour
{

    public static GlobalGameManager instance;
    
    [SerializeField]
    private bool isInCunicolo;

    public GameObject groundTileMap, wallTileMap, groundCunicoloTileMap, wallCunicoloTileMap;

    public GameObject groundLights, cunicoloLights;
    
    private GameObject selectedInteractableObj;

    public GameObject SelectedInteractableObj
    {
        get => selectedInteractableObj;
        set => selectedInteractableObj = value;
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
            //Necessario? Dipende come gestiamo livelli differenti
            //DontDestroyOnLoad(gameObject);
            
            instance = this;
            isInCunicolo = false;
            
            groundTileMap.SetActive(true);
            wallTileMap.SetActive(true);
            groundLights.SetActive(true);
            
            groundCunicoloTileMap.SetActive(false);
            wallCunicoloTileMap.SetActive(false);
            cunicoloLights.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TODO CHANGE THIS TO INTERACTION KEY
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (selectedInteractableObj)
            {
                selectedInteractableObj.GetComponent<I_Interactable>().Interact();
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

    public void InteractBotola()
    {
        if (selectedInteractableObj)
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
