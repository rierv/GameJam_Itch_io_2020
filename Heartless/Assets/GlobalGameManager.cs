using System.Collections;
using System.Collections.Generic;
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

    public GameObject firstLevel;
    public GameObject secondLevel;
    private GameObject currentLevel;
    
    [SerializeField]
    private GameObject selectedInteractableObj;

    public GameObject SelectedInteractableObj
    {
        get => selectedInteractableObj;
        set => selectedInteractableObj = value;
    }


    private int currentFloor;
    private LevelScript currentLevelScript;
    public GameObject globalLight;


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
            currentLevel = Instantiate(firstLevel);
            //secondLevel = Instantiate(secondLevel);
            //secondLevel.SetActive(false);
            
            currentLevelScript = currentLevel.GetComponent<LevelScript>();
            
            instance = this;
            IsInCunicolo = false;
            wasInCunicolo = false;
            playerInMirror = false;
            playerVisible = true;

            currentLevelScript.groundTileMap.SetActive(true);
            currentLevelScript.wallTileMap.SetActive(true);
            currentLevelScript.groundLights.SetActive(true);
            foreach (var go in currentLevelScript.objectsToHideInCunicolo)
            {
                go.SetActive(true);
            }
            
            currentLevelScript.groundCunicoloTileMap.SetActive(false);
            currentLevelScript.wallCunicoloTileMap.SetActive(false);
            currentLevelScript.cunicoloLights.SetActive(false);
            
            currentLevelScript.AStarObj.SetActive(true);
            currentLevelScript.enemyContainer.SetActive(true);
            
            //SET ENEMY TARGET
            foreach (Transform child in currentLevelScript.enemyContainer.transform)
            {
                child.gameObject.GetComponentInChildren<FSM_Enemy>().target = player;
            }
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
        currentLevelScript.cunicoloLights.SetActive(true);
        currentLevelScript.groundLights.SetActive(false);
        foreach (var go in currentLevelScript.objectsToHideInCunicolo)
        {
            go.SetActive(false);
        }

        foreach (Transform child in currentLevelScript.enemyContainer.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("noCollision"); //NO COLLISION
            SpriteRenderer sr = child.gameObject.GetComponentInChildren<SpriteRenderer>();
            sr.sortingLayerID = SortingLayer.NameToID("NemiciWhenInCunicolo");
            sr.color = new Color(0,0,0, 0.7f);
            foreach (Transform child2 in child.transform)
            {
                child2.gameObject.layer = LayerMask.NameToLayer("noCollision"); //NO COLLISION
            }
        }
        currentLevelScript.groundCunicoloTileMap.SetActive(true);
        currentLevelScript.wallCunicoloTileMap.SetActive(true);
        currentLevelScript.wallTileMap.GetComponent<TilemapCollider2D>().enabled = false;
    }

    private void ExitCunicolo()
    {
        IsInCunicolo = false;
        globalLight.GetComponent<Light2D>().intensity = 0.5f;
        currentLevelScript.cunicoloLights.SetActive(false);
        currentLevelScript.groundLights.SetActive(true);
        foreach (var go in currentLevelScript.objectsToHideInCunicolo)
        {
            go.SetActive(true);
        }
        foreach (Transform child in currentLevelScript.enemyContainer.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Enemy"); 
            SpriteRenderer sr = child.gameObject.GetComponentInChildren<SpriteRenderer>();
            sr.sortingLayerID = SortingLayer.NameToID("Player");
            sr.color = Color.white;
            foreach (Transform child2 in child.transform)
            {
                child2.gameObject.layer = LayerMask.NameToLayer("Enemy");
            }
        }
        currentLevelScript.groundCunicoloTileMap.SetActive(false);
        currentLevelScript.wallCunicoloTileMap.SetActive(false);
        currentLevelScript.wallTileMap.GetComponent<TilemapCollider2D>().enabled = true;
    }

    public void InteractBotola()
    {
        Debug.Log("GM interact botola");
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

            //player.GetComponent<PlayerController>().enabled = true;
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
        Debug.Log("Stop all coroutines");
        StopAllCoroutines();
        Debug.Log("AOOOOOO " + currentFloor);
        IsInCunicolo = false;
        wasInCunicolo = false;
        playerInMirror = false;
        playerVisible = true;
        player.GetComponent<PlayerController>().RestartLevel();
        
        //currentLevelScript.enemyContainer.SetActive(false);
        //currentLevelScript.AStarObj.SetActive(false);

        //currentLevel.SetActive(false);
        
        if (currentFloor == 1)
        {
            DestroyImmediate(currentLevel);
            GameObject nextLevel = Instantiate(secondLevel);
            currentLevel = nextLevel;
            currentFloor = 2;
        } else if (currentFloor == 2)
        {
            DestroyImmediate(currentLevel);
            GameObject nextLevel = Instantiate(firstLevel);
            currentLevel = nextLevel;
            currentFloor = 1;
        }
        
        currentLevel.SetActive(true);
        currentLevelScript = currentLevel.GetComponent<LevelScript>();
        
        
        currentLevelScript.groundTileMap.SetActive(true);
        currentLevelScript.wallTileMap.SetActive(true);
        currentLevelScript.groundLights.SetActive(true);
        foreach (var go in currentLevelScript.objectsToHideInCunicolo)
        {
            go.SetActive(true);
        }
            
        currentLevelScript.groundCunicoloTileMap.SetActive(false);
        currentLevelScript.wallCunicoloTileMap.SetActive(false);
        currentLevelScript.cunicoloLights.SetActive(false);
        
        //A* e Enemy Container DEVONO essere inactive in tutti i prefab dei livelli
        Debug.Log("Activate AStar");
        currentLevelScript.AStarObj.SetActive(true);
        Debug.Log("Activate Enemy Container");
        currentLevelScript.enemyContainer.SetActive(true);
        
        //SET ENEMY TARGET
        foreach (Transform child in currentLevelScript.enemyContainer.transform)
        {
            child.gameObject.GetComponentInChildren<FSM_Enemy>().target = player;
            child.gameObject.GetComponentInChildren<FSM_Enemy>().StartCoroutine("Patrol");
        }
    }
}
