using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionScript : MonoBehaviour
{

    PlayerInput input;

    Interactions interactions;

    InteractedScript npcInteraction;

    [SerializeField]
    GameObject inputField;

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

                inputField.SetActive(true);

                input.InGame.Complete.performed += ExitInputField;

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

        inputField.SetActive(true) ;

    }

    void ExitInputField(InputAction.CallbackContext context)
    {

        if(context.ReadValue<float>() == 1)
        {

            Debug.Log(npcInteraction);
            Debug.Log(inputField);

            npcInteraction.AskedAbout(inputField.GetComponent<TMP_InputField>().text);

            inputField.GetComponent<InputField>().text = "";

            inputField.SetActive(false);

            input.InGame.Complete.performed -= ExitInputField;

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

    private void FixedUpdate()
    {

        if(interactions.interact && interactions.canInteract)
        {

            transform.localScale = new Vector3(1.2f, 1.2f, 1);

        }
        else
        {

            transform.localScale = new Vector3(1f, 1, 1);

        }
        
    }

}
class Interactions
{

    public bool interact;
    public bool canInteract;

}
