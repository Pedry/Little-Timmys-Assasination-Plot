using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionScript : MonoBehaviour
{

    [SerializeField]
    playerController playerController;

    PlayerInput input;

    Interactions interactions;

    InteractedScript npcInteraction;

    [SerializeField]
    GameObject inputField;

    [SerializeField]
    GameObject outputField;

    private void Awake()
    {
    
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

        inputField.GetComponent<TMP_InputField>().ActivateInputField();

        playerController.DisableMovement();

    }

    void ExitInputField(InputAction.CallbackContext context)
    {

        if(context.ReadValue<float>() == 1)
        {

            npcInteraction.SetOutputField(outputField);
            npcInteraction.AskedAbout(inputField.GetComponent<TMP_InputField>().text);

            inputField.GetComponent<TMP_InputField>().text = "";

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
