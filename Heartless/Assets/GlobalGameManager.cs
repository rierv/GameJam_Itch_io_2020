using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

public class GlobalGameManager : MonoBehaviour
{

    public static GlobalGameManager instance;
    private GameObject player;

    public GameObject Player
    {
        get => player;
        set => player = value;
    }

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

    private bool playerInMirror;

    [SerializeField] 
    private bool playerVisible;

    public GameObject groundTileMap, wallTileMap, groundCunicoloTileMap, wallCunicoloTileMap;

    public GameObject groundLights, cunicoloLights;

    public GameObject globalLight;

    public GameObject[] groundObjects;

    public GameObject enemyContainer;
    
    [SerializeField]
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

            player = GameObject.FindWithTag("Player");
            
            instance = this;
            IsInCunicolo = false;
            playerInMirror = false;
            playerVisible = true;

            groundTileMap.SetActive(true);
            wallTileMap.SetActive(true);
            groundLights.SetActive(true);
            foreach (var go in groundObjects)
            {
                go.SetActive(true);
            }
            
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
        globalLight.GetComponent<Light2D>().intensity = 0.25f;
        cunicoloLights.SetActive(true);
        groundLights.SetActive(false);
        foreach (var go in groundObjects)
        {
            go.SetActive(false);
        }

        foreach (Transform child in enemyContainer.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("noCollision"); //NO COLLISION
            child.gameObject.GetComponentInChildren<SpriteRenderer>().sortingLayerID =
                SortingLayer.NameToID("NemiciWhenInCunicolo");
            foreach (Transform child2 in child.transform)
            {
                child2.gameObject.layer = LayerMask.NameToLayer("noCollision"); //NO COLLISION
            }
        }
        groundCunicoloTileMap.SetActive(true);
        wallCunicoloTileMap.SetActive(true);
        wallTileMap.GetComponent<TilemapCollider2D>().enabled = false;
    }

    private void ExitCunicolo()
    {
        IsInCunicolo = false;
        globalLight.GetComponent<Light2D>().intensity = 0.5f;
        cunicoloLights.SetActive(false);
        groundLights.SetActive(true);
        foreach (var go in groundObjects)
        {
            go.SetActive(true);
        }
        foreach (Transform child in enemyContainer.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Enemy"); 
            child.gameObject.GetComponentInChildren<SpriteRenderer>().sortingLayerID =
                SortingLayer.NameToID("Player");
            foreach (Transform child2 in child.transform)
            {
                child2.gameObject.layer = LayerMask.NameToLayer("Enemy");
            }
        }
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

            player.GetComponent<PlayerController>().enabled = true;
        }
    }

    public void InteractMirror()
    {
        if (playerInMirror)
        {
            SelectedInteractableObj.GetComponent<Animator>().SetTrigger("exitMirror");
            playerInMirror = false;
            playerVisible = true;
            player.SetActive(true);
        }
        else
        {
            SelectedInteractableObj.GetComponent<Animator>().SetTrigger("enterMirror");
            playerInMirror = true;
            playerVisible = false;
            GameObject selectedMirror = selectedInteractableObj;
            player.SetActive(false);
            selectedInteractableObj = selectedMirror;
            StartCoroutine(MirrorMaxTimeCoroutine());
        }
    }
    
    IEnumerator MirrorMaxTimeCoroutine()
    {
        //MIRROR MAX TIME = 5 SECONDI
        float maxTime = 5f;
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(maxTime);
        if (playerInMirror)
        {
            InteractMirror();
        }

    }
}
