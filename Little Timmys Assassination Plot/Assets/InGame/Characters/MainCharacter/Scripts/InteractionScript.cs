using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InteractionScript : MonoBehaviour
{

    GameObject engine;
    
    GameObject canvas;
    
    playerController playerController;

    PlayerInput input;

    Interactions interactions;

    InteractedScript npcInteraction;

    GameObject inputField;

    GameObject outputField;

    private void Awake()
    {

        playerController = null;
        inputField = null;
        outputField = null;

        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {

            if(obj.tag.Equals("Engine"))
            {

                engine = obj;

            }

        }

        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {

            if (obj.name.Contains("School"))
            {


                foreach(RectTransform obj2 in obj.GetComponentsInChildren<RectTransform>())
                {

                    if (obj2.gameObject.name.Contains("Canvas"))
                    {

                        canvas = obj.gameObject;

                    }

                }

            }

        }

        foreach (var obj in canvas.GetComponentsInChildren<RectTransform>())
        {

            if (obj.gameObject.name.Contains("Interact"))
            {

                inputField = obj.gameObject;

            }

        }

        inputField = GameObject.Find("InteractCanvas");


        foreach (var obj in canvas.GetComponentsInChildren<RectTransform>())
        {

            if (obj.gameObject.name.Contains("Output"))
            {

                outputField = obj.gameObject;

            }

        }


        playerController = GetComponent<playerController>();

        interactions = new Interactions();
        input = new PlayerInput();

        inputField.SetActive(false);
    
    }

    private void OnEnable()
    {
          
        input.Enable();

        input.InGame.Interact.performed += OnInteract;

    }

    private void OnDisable()
    {
        
        input.Disable();

    }

    void OnInteract(InputAction.CallbackContext context)
    {

        if(npcInteraction != null)
        {

            if (npcInteraction.GetComponent<StudentAnimation>().isMoving)
            {

                return;

            }

        }

        if(context.ReadValue<float>() == 1)
        {
            interactions.interact = true;

            if(npcInteraction != null)
            {

                npcInteraction.ChangeColor();

                input.InGame.Complete.performed += ExitInputField;

                input.InGame.Complete.Enable();

                EnterInputField();

            }

        }
        else
        {
            interactions.interact = false;
        }

    }

    void EnterInputField()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        inputField.SetActive(true);

        playerController.DisableMovement();

    }

    void ExitInputField(InputAction.CallbackContext context)
    {

        if(context.ReadValue<float>() == 1)
        {

            inputField.SetActive(false);

            input.InGame.Complete.performed -= ExitInputField;
            input.InGame.Complete.Disable();

            playerController.EnableMovement();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Student"))
        {

            npcInteraction = other.GetComponent<InteractedScript>();



            interactions.canInteract = true;

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Student"))
        {

            npcInteraction = null;

            interactions.canInteract = false;

        }

    }

}
class Interactions
{

    public bool interact;
    public bool canInteract;

}
