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
    private float movementSpeed = 10f;
    private Rigidbody2D rb2d;
    private Animator playerAnimator;
    private string lastAnimationTriggered;
    bool isAiming = false;
    float heartRadius = .6f;
    int heartCount = 1;
    // Start is called before the first frame update
    void Awake()
    {
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
        if (heartCount>0 && !isAiming && Input.GetKeyDown(KeyCode.K))
        {
            isAiming = true;
            CreateHeart();
            heartCount--;
        }
        if (isAiming)
        {
            Aim();
        }
        if (isAiming && Input.GetKeyUp(KeyCode.K))
        {
            isAiming = false;
            heartCount++;
            Destroy(spawnedHeart);
        }
        if (isAiming && Input.GetKeyUp(KeyCode.J))
        {
            isAiming = false;
            heartCount++;
            spawnedHeart.GetComponent<Heart>().Shoot(transform.position);
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
        inputMovementVector = (Vector2.right * Input.GetAxis("Horizontal") + Vector2.up * Input.GetAxis("Vertical")).normalized;
        if (inputMovementVector != Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + inputMovementVector, Time.deltaTime * movementSpeed);
        }
    }

    private void CreateHeart()
    {
        spawnedHeart = Instantiate(Heart, transform.position + Vector3.up, Quaternion.identity);
    }

    void Aim()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector3 WorldMouse = Camera.main.ScreenToWorldPoint(new Vector3(MousePos.x, MousePos.y, 0));
        Vector2 coord = new Vector2(WorldMouse.x - transform.position.x, WorldMouse.y - transform.position.y).normalized;
        Vector3 MouseOffset = transform.position + new Vector3(coord.x, coord.y / 2, 0) * heartRadius;



        spawnedHeart.transform.position = MouseOffset;

    }

}
