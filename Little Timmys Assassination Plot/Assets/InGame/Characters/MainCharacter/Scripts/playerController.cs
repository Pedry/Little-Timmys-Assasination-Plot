using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;

public class playerController : MonoBehaviour
{
    #region
    //H�r deklarerar jag variablar men s�tter inget v�rde p� dem. dela upp dem som "h�r" ihop! 
    #endregion
    [SerializeField]
    public float walkSpeed;
    [SerializeField]
    public float walkSpeedDiag;

    public bool walkRight = false;
    public bool walkLeft = false;
    public bool walkUp = false;
    public bool walkDown = true;

    public bool resetAnimation;
    GameObject engine;
    /* Jag vill anv�nda mig utav andra componenter eller program i unity. 
     * F�r att scriptet ska kunna f�rst� det
     * s� REFERERAR jag det genom att skriva namnet. OBS: Case Sensitive!
     * 
     * VIKTIGT!N�r vi refererat n�got betyder inte det att vi kan anv�nda det �n.
     * Vi har bara gjort att programmet vet om att det finns. Resten g�r vi senare!
     */


    #region
    // "PlayerInput" �r namnet p� mitt "Input System" som jag installerade i Packet Managern.
    #endregion
    PlayerInput input;


    /* "Rigidbodyy2D" �r namnet p� komponenten i unity som jag vill kunna komma �t och anv�nda.
     * Alla gameObject som jag s�tter detta hela scriptet p� m�ste ha en Rigidbody2D.
     */
    Rigidbody2D rb;

    SpriteLibrary spriteLibrary;
    SpriteRenderer spriteRenderer;
    new GameObject camera;

    #region
    // Start is called before the first frame update
    #endregion
    void Start()
    {
        
    }

    /*
     *  Awake is called when the script object is initialised, 
     *  regardless of whether or not the script is enabled.
     */
    private void Awake()
    {

        camera = Camera.main.gameObject;

        engine = null;

        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {

            if (obj.tag.Equals("Engine"))
            {

                engine = obj;

            }

        }

        #region
        //I v�ra scopes s� b�rjar vi s�tta v�rde p� v�ra variablar.
        #endregion

        walkRight = false;

        resetAnimation = false;

        #region
        //H�gst upp s� REFERERADE vi de komponenter som vi vill kunna anv�nda.
        //H�r g�r vi s� att vi faktiskt kan anv�nda dem!
        #endregion
        rb = GetComponent<Rigidbody2D>();

        spriteLibrary = GetComponent<SpriteLibrary>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        #region
        //H�r skriver vi namnet vi gav till referensen och skriver "new PlayerInput();"
        //Anledningen till att vi skriver det �r s� att vi kan �ndra p� PlayerInput scriptet 
        //Utan att beh�va g� in i det. Vi kan nu g�ra alla �ndringar h�r direkt och v�ra �ndringar 
        //Kommer �ven �ndras i PlayerInput Scriptet ocks�! P� detta s�ttet beh�ver vi aldrig ens g� in i det andra scriptet. Vi kan h�lla oss h�r!
        #endregion
        input = new PlayerInput();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnEnable()
    {
        
        input.Enable();
        #region
        //+= betyder att vi l�gger till allt p� h�ger sida p� v�nster sida.
        #endregion

        input.InGame.WalkRight.performed += OnWalkRight;
        input.InGame.WalkLeft.performed += OnWalkLeft;
        input.InGame.WalkUp.performed += OnWalkUp;
        input.InGame.WalkDown.performed += OnWalkDown;
        input.InGame.QuitGame.performed += QuitGame;
    }

    void QuitGame(InputAction.CallbackContext context)
    {
        
        Application.Quit();
        
    }

    public void DisableMovement()
    {

        input.InGame.WalkDown.Disable();
        input.InGame.WalkLeft.Disable();
        input.InGame.WalkUp.Disable();
        input.InGame.WalkRight.Disable();

    }

    public void EnableMovement()
    {

        input.InGame.WalkDown.Enable();
        input.InGame.WalkLeft.Enable();
        input.InGame.WalkUp.Enable();
        input.InGame.WalkRight.Enable();

    }

    [BurstCompile]
    private void OnDisable()
    {
        input.Disable();
    }

    [BurstCompile]
    void FixedUpdate()
    {
        MoveRight(); 
        MoveLeft();
        MoveUp();
        MoveDown();

    }

    [BurstCompile]
    private void Update()
    {

        camera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, camera.transform.position.z);

        UpdateSprite();

    }

    #region
    //Funktionen f�r att flytta oss i h�ger riktning.
    #endregion
    [BurstCompile]
    void MoveRight()
    {
        if (rb.velocity.x > 0.1f)
        {

            rb.velocity = new Vector2(rb.velocity.x - 20f, rb.velocity.y);

        }

        if (walkRight)
        {

            if(walkUp || walkDown)
            {

                rb.velocity = new Vector2(walkSpeedDiag, rb.velocity.y);

            }
            else
            {

                rb.velocity = new Vector2(walkSpeed, rb.velocity.y);

            }
        }
    }

    #region
    //Funktionen f�r att flytta oss i v�nster riktning.
    #endregion
    [BurstCompile]
    void MoveLeft()
    {
        if (rb.velocity.x < -0.1f)
        {  
            rb.velocity = new Vector2(rb.velocity.x + 20f, rb.velocity.y);
        }

        if (walkLeft)
        {
            if(walkUp || walkDown)
            {

                rb.velocity = new Vector2(-walkSpeedDiag, rb.velocity.y);

            }
            else
            {

                rb.velocity = new Vector2(-walkSpeed, rb.velocity.y);

            }
        }
    }

    #region
    //Funktionen f�r att flytta oss i upp�t riktning.
    #endregion
    [BurstCompile]
    void MoveUp()
    {
        if (rb.velocity.y > 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 20f);
        }

        if (walkUp)
        {

            if(walkLeft || walkRight)
            {

                rb.velocity = new Vector2(rb.velocity.x, walkSpeedDiag);

            }
            else
            {

                rb.velocity = new Vector2(rb.velocity.x, walkSpeed);

            }
        }
    }

    #region
    //Funktionen f�r att flytta oss i ner�t riktning.
    #endregion
    [BurstCompile]
    void MoveDown()
    {
        if (rb.velocity.y < -0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + 20f);
        }

        if (walkDown)
        {

            if (walkLeft || walkRight)
            {

                rb.velocity = new Vector2(rb.velocity.x, -walkSpeedDiag);

            }
            else
            {

                rb.velocity = new Vector2(rb.velocity.x, -walkSpeed);

            }
        }
    }

    [BurstCompile]
    void OnWalkRight(InputAction.CallbackContext context)
    {

        if (context.ReadValue<float>() == 1)
        {
            walkRight = true;
            resetAnimation = true;

            spriteRenderer.flipX = true;

        }
        if (context.ReadValue<float>() == 0)
        {
            walkRight = false;
        }

    }
    [BurstCompile]
    void OnWalkLeft(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1)
        {
            walkLeft = true;
            resetAnimation = true;

            spriteRenderer.flipX = false;
        }
        if (context.ReadValue<float>() == 0)
        {
            walkLeft = false;
        }
    }
    [BurstCompile]
    void OnWalkUp(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1)
        {
            walkUp = true;
            resetAnimation = true;
        }
        if (context.ReadValue<float>() == 0)
        {
            walkUp = false;
        }
    }
    [BurstCompile]
    void OnWalkDown(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1)
        {
            walkDown = true;
            resetAnimation = true;

        }
        if (context.ReadValue<float>() == 0)
        {
            walkDown = false;
        }
    }

    public string lastCategory = "Down";
    public int lastFrameOffset = 0;

    [BurstCompile]
    void UpdateSprite()
    {
        string label = "timothy-animated-sprite 1_";

        spriteRenderer.sprite = spriteLibrary.GetSprite(lastCategory, label + lastFrameOffset);

        if (resetAnimation)
        {

            engine.GetComponent<AnimationEngineScript>().ResetTimmyAnimation();
            resetAnimation = false;

        }

        if (walkUp && walkLeft)
        {

            spriteRenderer.sprite = spriteLibrary.GetSprite("UpDiag", label + (engine.GetComponent<AnimationEngineScript>().timmyAnimationFrame + 16));
            lastCategory = "UpDiag";
            lastFrameOffset = 16;
            return;

        }

        if(walkDown && walkLeft)
        {

            spriteRenderer.sprite = spriteLibrary.GetSprite("DownDiag", label + (engine.GetComponent<AnimationEngineScript>().timmyAnimationFrame + 12));
            lastCategory = "DownDiag";
            lastFrameOffset = 12;
            return;

        }

        if(walkUp && walkRight)
        {

            spriteRenderer.sprite = spriteLibrary.GetSprite("UpDiag", label + (engine.GetComponent<AnimationEngineScript>().timmyAnimationFrame + 16));
            lastCategory = "UpDiag";
            lastFrameOffset = 16;
            return;

        }

        if(walkDown && walkRight)
        {

            spriteRenderer.sprite = spriteLibrary.GetSprite("DownDiag", label + (engine.GetComponent<AnimationEngineScript>().timmyAnimationFrame + 12));
            lastCategory = "DownDiag";
            lastFrameOffset = 12;
            return;

        }

        if (walkLeft)
        {

            spriteRenderer.sprite = spriteLibrary.GetSprite("Horizontal", label + (engine.GetComponent<AnimationEngineScript>().timmyAnimationFrame + 4));
            lastCategory = "Horizontal";
            lastFrameOffset = 4;
            return;

        }

        if(walkUp)
        {

            spriteRenderer.sprite = spriteLibrary.GetSprite("Up", label + (engine.GetComponent<AnimationEngineScript>().timmyAnimationFrame + 8));
            lastCategory = "Up";
            lastFrameOffset = 8;
            return;

        }

        if (walkDown)
        {

            spriteRenderer.sprite = spriteLibrary.GetSprite("Down", label + engine.GetComponent<AnimationEngineScript>().timmyAnimationFrame);
            lastCategory = "Down";
            lastFrameOffset = 0;
            return;

        }

        if (walkRight)
        {

            spriteRenderer.sprite = spriteLibrary.GetSprite("Horizontal", label + (engine.GetComponent<AnimationEngineScript>().timmyAnimationFrame + 4));
            lastCategory = "Horizontal";
            lastFrameOffset = 4;
            return;

        }
    }


}
