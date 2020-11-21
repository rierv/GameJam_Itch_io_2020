using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

public class GlobalGameManager : MonoBehaviour
{

    public static GlobalGameManager instance;
    
    [SerializeField]
    private bool isInCunicolo;

    public bool IsInCunicolo
    {
        get => isInCunicolo;
        set => isInCunicolo = value;
    }

    public bool PlayerVisible
    {
        get => playerVisible;
        set => playerVisible = value;
    }

    [SerializeField] 
    private bool playerVisible;

    public GameObject groundTileMap, wallTileMap, groundCunicoloTileMap, wallCunicoloTileMap;

    public GameObject groundLights, cunicoloLights;

    public GameObject globalLight;
    
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
            IsInCunicolo = false;
            
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
        IsInCunicolo = true;
        GameObject.FindWithTag("Player").GetComponent<Animator>().SetTrigger("enterBotola");
        globalLight.GetComponent<Light2D>().intensity = 0.25f;
        cunicoloLights.SetActive(true);
        groundLights.SetActive(false);
        groundCunicoloTileMap.SetActive(true);
        wallCunicoloTileMap.SetActive(true);
        wallTileMap.GetComponent<TilemapCollider2D>().enabled = false;
    }

    private void ExitCunicolo()
    {
        IsInCunicolo = false;
        GameObject.FindWithTag("Player").GetComponent<Animator>().SetTrigger("enterBotola");
        globalLight.GetComponent<Light2D>().intensity = 0.5f;
        cunicoloLights.SetActive(false);
        groundLights.SetActive(true);
        groundCunicoloTileMap.SetActive(false);
        wallCunicoloTileMap.SetActive(false);
        wallTileMap.GetComponent<TilemapCollider2D>().enabled = true;
    }

    public void InteractBotola()
    {
        if (selectedInteractableObj)
        {
            if (IsInCunicolo)
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
