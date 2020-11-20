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
    
    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    public void OnMove(InputValue value)
    {
        inputMovementVector = value.Get<Vector2>();
    }

    private void MovePlayer()
    {
        if (inputMovementVector != Vector2.zero)
        {
            Vector2 movementInput = new Vector2(inputMovementVector.x, inputMovementVector.y).normalized * Time.deltaTime;
            transform.Translate(movementInput * movementSpeed); 
        }
    }
}
