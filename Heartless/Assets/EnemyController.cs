﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
   
    private Vector3 inputMovementVector;
    [SerializeField]
    private float movementSpeed = 10f;
    private Rigidbody2D rb2d;
    private Animator enemyAnimator;
    private string lastAnimationTriggered;
    bool isAiming = false;
    public bool stunned = false;

    public void SetStunned(bool value)
    {
        if (value && !stunned)
        {
            enemyAnimator.SetTrigger("hitted");
        }
        stunned = value;
    }
    
    private Vector3 lastPosition;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        lastAnimationTriggered = "idle";
        lastPosition = transform.parent.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //ANIMATION 
        Vector3 movementVector = (transform.position - lastPosition).normalized;
        SetAnimation(movementVector);
        lastPosition = transform.position;
    }
    
    private void SetAnimation(Vector2 input)
    {
        if (stunned)
        {
            return;
        }
        
        
        if (input == Vector2.zero)
        {
            //EDIT HERE IF WE WANT A DIFFERENT IDLE ANIMATION BASED ON LAST MOVEMENT DIRECTION
            //IDLE
            /*
            if(stunned&&lastAnimationTriggered != "hitted")
            {
                Debug.Log("hittedf");
                enemyAnimator.SetTrigger("hitted");
                lastAnimationTriggered = "hitted";
            }
            */
            if (!stunned&&lastAnimationTriggered != "idle")
            {
                enemyAnimator.SetTrigger("idle");
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
                        enemyAnimator.SetTrigger("walkUp");
                        lastAnimationTriggered = "walkUp";
                    }
                }
                else
                {
                    //DOWN
                    if (lastAnimationTriggered != "walkDown")
                    {
                        enemyAnimator.SetTrigger("walkDown");
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
                        enemyAnimator.SetTrigger("walkRight");
                        lastAnimationTriggered = "walkRight";
                    }
                }
                else
                {
                    //LEFT
                    if (lastAnimationTriggered != "walkLeft")
                    {
                        enemyAnimator.SetTrigger("walkLeft");
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

}
