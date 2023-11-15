using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    [SerializeField]
    float walkSpeed;

    bool walkRight;
    bool walkLeft;
    bool walkUp;
    bool walkDown;

    PlayerInput input;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        walkRight = false;
        rb = GetComponent<Rigidbody2D>();
        input = new PlayerInput();
    }
    private void OnEnable()
    {
        input.Enable();

        input.InGame.WalkRight.performed += OnWalkRight;
        input.InGame.WalkLeft.performed += OnWalkLeft;
        input.InGame.WalkUp.performed += OnWalkUp;
        input.InGame.WalkDown.performed += OnWalkDown;
    }
    private void OnDisable()
    {
        input.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveRight(); 
        MoveLeft();

    }
    void MoveRight()
    {
        if (rb.velocity.x > 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x - 1f, rb.velocity.y);
        }

        if (walkRight)
        {
            rb.velocity = new Vector2(walkSpeed, 0);
        }
    }
    void MoveLeft()
    {
        if (rb.velocity.x < -0.1f)
        {  
            rb.velocity = new Vector2(rb.velocity.x + 1f, rb.velocity.y);
        }

        if (walkLeft)
        {
            rb.velocity = new Vector2(-walkSpeed, 0);
        }
    }

    void OnWalkRight(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1)
        {
            walkRight = true;
        }
        if (context.ReadValue<float>() == 0)
        {
            walkRight = false;
        }

    }
    void OnWalkLeft(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1)
        {
            walkLeft = true;
        }
        if (context.ReadValue<float>() == 0)
        {
            walkLeft = false;
        }
    }
    void OnWalkUp(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1)
        {
            walkUp = true;
        }
        if (context.ReadValue<float>() == 0)
        {
            walkUp = false;
        }
    }
    void OnWalkDown(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1)
        {
            walkDown = true;
        }
        if (context.ReadValue<float>() == 0)
        {
            walkDown = false;
        }
    }


}
