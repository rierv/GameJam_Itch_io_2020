using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


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
    float heartRadius = .6f;
    int heartCount = 1;
    public int startHeartCount = 1;
    Vector3 startPosition;
    GlobalGameManager GameManager;
    // Start is called before the first frame update
    void Awake()
    {
        GameManager = GameObject.FindObjectOfType<GlobalGameManager>();
        startPosition = transform.position;
        startHeartCount = heartCount;
        rb2d = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        lastAnimationTriggered = "idle";
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
    }

    void Aim()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector3 WorldMouse = Camera.main.ScreenToWorldPoint(new Vector3(MousePos.x, MousePos.y, 0));
        Vector2 coord = new Vector2(WorldMouse.x - transform.position.x, WorldMouse.y - transform.position.y).normalized;
        Vector3 MouseOffset = transform.position + new Vector3(coord.x, coord.y / 1.5f, 0) * heartRadius;
        if(coord.y<0) spawnedHeart.GetComponent<SpriteRenderer>().sortingOrder = 10;
        else spawnedHeart.GetComponent<SpriteRenderer>().sortingOrder = -10;



        spawnedHeart.transform.position = Vector3.Lerp(spawnedHeart.transform.position, MouseOffset, Time.deltaTime * 5);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Heart")
        {
            Destroy(collision.gameObject);
            heartCount++;
        }
        if (collision.gameObject.tag == "Enemy")
        {

            if(!collision.gameObject.GetComponent<EnemyController>().stunned&& GameManager.PlayerVisible && !GameManager.IsInCunicolo)
                RestartLevel();
        }
    }

    private void NotifyGGMBotolaInteraction()
    {
        //Questo metodo viene chiamato alla fine dell'animazione "EnterBotola"
        GlobalGameManager.instance.InteractBotola();
    }
    void RestartLevel()
    {
        
        foreach (Heart h in FindObjectsOfType<Heart>())
        {
            Destroy(h.gameObject);
        }
        foreach (EnemyController ep in FindObjectsOfType<EnemyController>())
        {
            ep.gameObject.transform.localPosition = Vector3.zero;
        }
        startHeartCount = startHeartCount + 1;
        heartCount = startHeartCount;
        transform.position = startPosition;
    }
}
