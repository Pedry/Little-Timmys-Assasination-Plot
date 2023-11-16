using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    //Här deklarerar jag variablar men sätter inget värde på dem. dela upp dem som "hör" ihop! 
    [SerializeField]
    float walkSpeed;

    bool walkRight;
    bool walkLeft;
    bool walkUp;
    bool walkDown;
    /* Jag vill använda mig utav andra componenter eller program i unity. 
     * För att scriptet ska kunna förstå det
     * så REFERERAR jag det genom att skriva namnet. OBS: Case Sensitive!
     * 
     * VIKTIGT!När vi refererat något betyder inte det att vi kan använda det än.
     * Vi har bara gjort att programmet vet om att det finns. Resten gör vi senare!
     */
    
     
     
    // "PlayerInput" är namnet på mitt "Input System" som jag installerade i Packet Managern.
    PlayerInput input;


    /* "Rigidbodyy2D" är namnet på komponenten i unity som jag vill kunna komma åt och använda.
     * Alla gameObject som jag sätter detta hela scriptet på måste ha en Rigidbody2D.
     */
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /*
     *  Awake is called when the script object is initialised, 
     *  regardless of whether or not the script is enabled.
     */
    private void Awake()
    {
        //I våra scopes så börjar vi sätta värde på våra variablar.
        walkRight = false;

        //Högst upp så REFERERADE vi de komponenter som vi vill kunna använda.
        //Här gör vi så att vi faktiskt kan använda dem!
        rb = GetComponent<Rigidbody2D>();

        //Här skriver vi namnet vi gav till referensen och skriver "new PlayerInput();"
        //Anledningen till att vi skriver det är så att vi kan ändra på PlayerInput scriptet 
        //Utan att behöva gå in i det. Vi kan nu göra alla ändringar här direkt och våra ändringar 
        //Kommer även ändras i PlayerInput Scriptet också! På detta sättet behöver vi aldrig ens gå in i det andra scriptet. Vi kan hålla oss här!
        input = new PlayerInput();
    }
    private void OnEnable()
    {
        
        input.Enable();
        //+= betyder att vi lägger till allt på höger sida på vänster sida.
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
        MoveUp();
        MoveDown();

    }

    //Funktionen för att flytta oss i höger riktning.
    void MoveRight()
    {
        if (rb.velocity.x > 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x - 1f, rb.velocity.y);
        }

        if (walkRight)
        {
            rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
        }
    }

    //Funktionen för att flytta oss i vänster riktning.
    void MoveLeft()
    {
        if (rb.velocity.x < -0.1f)
        {  
            rb.velocity = new Vector2(rb.velocity.x + 1f, rb.velocity.y);
        }

        if (walkLeft)
        {
            rb.velocity = new Vector2(-walkSpeed, rb.velocity.y);
        }
    }

    //Funktionen för att flytta oss i uppåt riktning.
    void MoveUp()
    {
        if (rb.velocity.y > 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 1f );
        }

        if (walkUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, walkSpeed);
        }
    }

    //Funktionen för att flytta oss i neråt riktning.
    void MoveDown()
    {
        if (rb.velocity.y < -0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + 1f);
        }

        if (walkDown)
        {
            rb.velocity = new Vector2(rb.velocity.x, -walkSpeed);
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
