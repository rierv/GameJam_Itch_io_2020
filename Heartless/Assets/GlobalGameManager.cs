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

    private bool wasInCunicolo;

    public bool WasInCunicolo
    {
        get => wasInCunicolo;
        set => wasInCunicolo = value;
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
    
    public GameObject level1Container, level2Container;
    public GameObject groundTileMap2, walltileMap2, groundCunicoloTileMap2, wallCunicoloTileMap2;
    public GameObject groundLights2, cunicoloLights2;
    public GameObject[] groundObjects2;
    public GameObject enemyContainer2;

    private GameObject groundTileMap1, wallTileMap1, groundCunicoloTileMap1, wallCunicoloTilemap1;
    private GameObject groundLights1, cunicoloLights1;
    private GameObject[] groundObjects1;
    private GameObject enemyContainer1;
    
    [SerializeField]
    private GameObject selectedInteractableObj;

    public GameObject SelectedInteractableObj
    {
        get => selectedInteractableObj;
        set => selectedInteractableObj = value;
    }


    private int currentFloor;

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
            player = GameObject.FindWithTag("Player");
            currentFloor = 1;
            
            instance = this;
            IsInCunicolo = false;
            wasInCunicolo = false;
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
            
            //SET ENEMY TARGET
            foreach (Transform child in enemyContainer.transform)
            {
                child.gameObject.GetComponentInChildren<FSM_Enemy>().target = player;
            }

            groundTileMap1 = groundTileMap;
            wallTileMap1 = wallTileMap;
            groundCunicoloTileMap1 = groundCunicoloTileMap;
            wallCunicoloTilemap1 = wallCunicoloTileMap;
            groundLights1 = groundLights;
            cunicoloLights1 = cunicoloLights;
            groundObjects1 = groundObjects;
            enemyContainer1 = enemyContainer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TODO CHANGE THIS TO INTERACTION KEY
        if (Input.GetKeyDown(KeyCode.Space))
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
            if (WasInCunicolo)
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

    public void SwitchFloor()
    {
        Debug.Log("AOOOOOO " + currentFloor);
        IsInCunicolo = false;
        wasInCunicolo = false;
        playerInMirror = false;
        playerVisible = true;
        
        if (currentFloor == 1)
        {
            level1Container.SetActive(false);
            level2Container.SetActive(true);
            groundTileMap = groundTileMap2;
            wallTileMap = walltileMap2;
            groundCunicoloTileMap = groundCunicoloTileMap2;
            wallCunicoloTileMap = wallCunicoloTileMap2;
            groundObjects = groundObjects2;
        
            groundLights = groundLights2;
            cunicoloLights = cunicoloLights2;
        
            enemyContainer = enemyContainer2;
        } 
        else if (currentFloor == 2)
        {
            level2Container.SetActive(false);
            level1Container.SetActive(true);

            groundTileMap = groundTileMap1;
            wallTileMap = wallTileMap1;
            groundCunicoloTileMap = groundCunicoloTileMap1;
            wallCunicoloTileMap = wallCunicoloTilemap1;
            groundObjects = groundObjects1;
        
            groundLights = groundLights1;
            cunicoloLights = cunicoloLights1;
        
            enemyContainer = enemyContainer1;
        }
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

        //SET ENEMY TARGET
        foreach (Transform child in enemyContainer.transform)
        {
            child.gameObject.GetComponentInChildren<FSM_Enemy>().target = player;
        }

        if (currentFloor == 1)
        {
            currentFloor = 2;
        }
        else
        {
            currentFloor = 1;
        }
    }

    public void GoToLevel2()
    {
        Debug.Log("AOOOOOOO");
        level1Container.SetActive(false);
        level2Container.SetActive(true);
        
        IsInCunicolo = false;
        wasInCunicolo = false;
        playerInMirror = false;
        playerVisible = true;

        groundTileMap = groundTileMap2;
        wallTileMap = walltileMap2;
        groundCunicoloTileMap = groundCunicoloTileMap2;
        wallCunicoloTileMap = wallCunicoloTileMap2;
        groundObjects = groundObjects2;
        
        groundLights = groundLights2;
        cunicoloLights = cunicoloLights2;
        
        enemyContainer = enemyContainer2;

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

        //SET ENEMY TARGET
        foreach (Transform child in enemyContainer.transform)
        {
            child.gameObject.GetComponentInChildren<FSM_Enemy>().target = player;
        }
    }
}
