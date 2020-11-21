using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private Vector2 inputMovementVector;
    [SerializeField]
    private float movementSpeed = 3f;
    private Rigidbody2D rb2d;
    private Animator playerAnimator;
    private string lastAnimationTriggered;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        lastAnimationTriggered = "idle";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
        //ANIMATION 
        SetAnimation(inputMovementVector);
    }

    public void OnMove(InputValue value)
    {
        inputMovementVector = value.Get<Vector2>();
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
        if (inputMovementVector != Vector2.zero)
        {
            //MOVEMENT
            Vector2 movementInput = new Vector2(inputMovementVector.x, inputMovementVector.y).normalized * Time.deltaTime;
            transform.Translate(movementInput * movementSpeed); 
        }
    }
}
