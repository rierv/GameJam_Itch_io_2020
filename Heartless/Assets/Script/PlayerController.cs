using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject Heart;
    private GameObject spawnedHeart;
    private Vector3 inputMovementVector;
    [SerializeField]
    private float movementSpeed = 100f;
    private Rigidbody2D rb2d;
    private Animator playerAnimator;
    private string lastAnimationTriggered;
    bool isAiming = false;
    float heartRadius = 1f;
    int heartCount = 1;
    public int startHeartCount = 1;
    public Vector3 startPosition;
    GlobalGameManager GameManager;
    Text txtHearts;
    bool dead=false;
    public AudioClip soundDie;
    // Start is called before the first frame update
    void Awake()
    {
        txtHearts = GetComponentInChildren<Text>();
        GameManager = GameObject.FindObjectOfType<GlobalGameManager>();
        startPosition = transform.position;
        heartCount = startHeartCount;
        rb2d = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        lastAnimationTriggered = "idle";
        txtHearts.text = startHeartCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        //ANIMATION 
        SetAnimation(inputMovementVector);
        if (heartCount>0 && !isAiming && Input.GetMouseButtonDown(0))
        {
            isAiming = true;
            CreateHeart();
            heartCount--;
            txtHearts.text = heartCount.ToString();
        }
        if (isAiming)
        {
            if (spawnedHeart && spawnedHeart.GetComponent<Heart>().enemy == null) Aim();
            else isAiming = false;
        }
        if (isAiming && Input.GetMouseButtonUp(0))
        {
            isAiming = false;
            heartCount++;
            txtHearts.text = heartCount.ToString();
            Destroy(spawnedHeart);
        }
        if (isAiming && Input.GetMouseButtonDown(1))
        {
            isAiming = false;
            spawnedHeart.GetComponent<Heart>().Shoot(spawnedHeart.transform.position - transform.position);
        }
    }

    

    private void SetAnimation(Vector2 input)
    {
        if (input == Vector2.zero)
        {
            //EDIT HERE IF WE WANT A DIFFERENT IDLE ANIMATION BASED ON LAST MOVEMENT DIRECTION
            //IDLE
            if (lastAnimationTriggered != "idle")
            {
                playerAnimator.SetTrigger("idle");
                lastAnimationTriggered = "idle";
            }
        }
        else
        {
            if (Mathf.Abs(input.y) >= Mathf.Abs(input.x))
            {
                //VERTICAL MOVEMENT
                if (input.y >= 0)
                {
                    //UP
                    if (lastAnimationTriggered != "walkUp")
                    {
                        playerAnimator.SetTrigger("walkUp");
                        lastAnimationTriggered = "walkUp";
                    }
                }
                else
                {
                    //DOWN
                    if (lastAnimationTriggered != "walkDown")
                    {
                        playerAnimator.SetTrigger("walkDown");
                        lastAnimationTriggered = "walkDown";
                    }
                }
            }
            else
            {
                //HORIZONTAL MOVEMENT
                if (input.x >= 0)
                {
                    //RIGHT
                    if (lastAnimationTriggered != "walkRight")
                    {
                        playerAnimator.SetTrigger("walkRight");
                        lastAnimationTriggered = "walkRight";
                    }
                }
                else
                {
                    //LEFT
                    if (lastAnimationTriggered != "walkLeft")
                    {
                        playerAnimator.SetTrigger("walkLeft");
                        lastAnimationTriggered = "walkLeft";
                    }
                }
            }
        }
    }

    private void MovePlayer()
    {
        inputMovementVector = (Vector2.right * Input.GetAxis("Horizontal") + Vector2.up * Input.GetAxis("Vertical"));
        transform.position = Vector3.Lerp(transform.position, transform.position + inputMovementVector.normalized, Time.deltaTime * movementSpeed);
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + inputMovementVector.normalized/10, Time.deltaTime*movementSpeed);

    }

    private void CreateHeart()
    {
        spawnedHeart = Instantiate(Heart, transform.position, Quaternion.identity);
        //spawnedHeart.GetComponent<Heart>().readyToHit = true;
    }

    void Aim()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector3 WorldMouse = Camera.main.ScreenToWorldPoint(new Vector3(MousePos.x, MousePos.y, 0));
        Vector2 coord = new Vector2(WorldMouse.x - transform.position.x, WorldMouse.y - transform.position.y).normalized;
        Vector3 MouseOffset = transform.position + new Vector3(coord.x, coord.y / 1.5f, 0) * heartRadius;
        if(coord.y<0) spawnedHeart.GetComponent<SpriteRenderer>().sortingOrder = 10;
        else spawnedHeart.GetComponent<SpriteRenderer>().sortingOrder = -10;



        spawnedHeart.transform.position = Vector3.Lerp(spawnedHeart.transform.position, MouseOffset, Time.deltaTime * 20);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Heart")
        {
            Destroy(collision.gameObject);
            heartCount++;
            txtHearts.text = heartCount.ToString();
            if(!isAiming|| spawnedHeart.GetComponent<Heart>().enemy==null) GetComponent<AudioSource>().Play();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Collision w/ Enemy");
            if(!collision.gameObject.GetComponent<EnemyController>().stunned&& GameManager.PlayerVisible && !GameManager.IsInCunicolo)
            {
                //PLAYER DIE
                GetComponent<CapsuleCollider2D>().enabled = false;
                playerAnimator.SetTrigger("die");
                GetComponent<AudioSource>().PlayOneShot(soundDie);
                this.enabled = false;
                dead = true;
                /*
                //ANIMATION WILL CALL PlayerDie()
                startHeartCount = startHeartCount + 1;
                RestartLevel();
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GlobalGameManager.instance.currentFloor = GlobalGameManager.instance.currentFloor % 2 + 1;
                GlobalGameManager.instance.SwitchFloor();
                */
            }
        }
    }

    private void PlayerDie()
    {
        startHeartCount = startHeartCount + 1;
        RestartLevel();
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GlobalGameManager.instance.SwitchFloor(GlobalGameManager.instance.currentFloor);
    }

    private void NotifyGGMBotolaInteraction()
    {
        //Questo metodo viene chiamato alla fine dell'animazione "EnterBotola"
        //Debug.Log("NOTIFY GM Botola");
        GlobalGameManager.instance.InteractBotola();
    }
    
    private void EnablePlayerController()
    {
        Debug.Log("Enable Player Controller");
        this.enabled = true;
    }

    public void DisablePlayerCollider()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        //GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void EnablePlayerCollider()
    {
        GetComponent<CapsuleCollider2D>().enabled = true;
        //GetComponent<Rigidbody2D>().isKinematic = false;

    }
    
    public void RestartLevel()
    {
        
        foreach (Heart h in FindObjectsOfType<Heart>())
        {
            Destroy(h.gameObject);
        }
        
        transform.position = startPosition;
        heartCount = startHeartCount;
        txtHearts.text = heartCount.ToString();
        if (dead)
        {
            GetComponent<CapsuleCollider2D>().enabled = true;
            playerAnimator.SetTrigger("respawn");
            dead = false;
        }
        this.enabled = false;
    }
}
