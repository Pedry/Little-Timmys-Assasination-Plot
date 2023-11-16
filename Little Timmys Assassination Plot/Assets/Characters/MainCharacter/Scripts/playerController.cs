using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    //H�r deklarerar jag variablar men s�tter inget v�rde p� dem. dela upp dem som "h�r" ihop! 
    [SerializeField]
    float walkSpeed;

    bool walkRight;
    bool walkLeft;
    bool walkUp;
    bool walkDown;
    /* Jag vill anv�nda mig utav andra componenter eller program i unity. 
     * F�r att scriptet ska kunna f�rst� det
     * s� REFERERAR jag det genom att skriva namnet. OBS: Case Sensitive!
     * 
     * VIKTIGT!N�r vi refererat n�got betyder inte det att vi kan anv�nda det �n.
     * Vi har bara gjort att programmet vet om att det finns. Resten g�r vi senare!
     */
    
     
     
    // "PlayerInput" �r namnet p� mitt "Input System" som jag installerade i Packet Managern.
    PlayerInput input;


    /* "Rigidbodyy2D" �r namnet p� komponenten i unity som jag vill kunna komma �t och anv�nda.
     * Alla gameObject som jag s�tter detta hela scriptet p� m�ste ha en Rigidbody2D.
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
        //I v�ra scopes s� b�rjar vi s�tta v�rde p� v�ra variablar.
        walkRight = false;

        //H�gst upp s� REFERERADE vi de komponenter som vi vill kunna anv�nda.
        //H�r g�r vi s� att vi faktiskt kan anv�nda dem!
        rb = GetComponent<Rigidbody2D>();

        //H�r skriver vi namnet vi gav till referensen och skriver "new PlayerInput();"
        //Anledningen till att vi skriver det �r s� att vi kan �ndra p� PlayerInput scriptet 
        //Utan att beh�va g� in i det. Vi kan nu g�ra alla �ndringar h�r direkt och v�ra �ndringar 
        //Kommer �ven �ndras i PlayerInput Scriptet ocks�! P� detta s�ttet beh�ver vi aldrig ens g� in i det andra scriptet. Vi kan h�lla oss h�r!
        input = new PlayerInput();
    }
    private void OnEnable()
    {
        
        input.Enable();
        //+= betyder att vi l�gger till allt p� h�ger sida p� v�nster sida.
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

    //Funktionen f�r att flytta oss i h�ger riktning.
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

    //Funktionen f�r att flytta oss i v�nster riktning.
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

    //Funktionen f�r att flytta oss i upp�t riktning.
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

    //Funktionen f�r att flytta oss i ner�t riktning.
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
