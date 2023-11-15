using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionScript : MonoBehaviour
{

    PlayerInput input;

    Interactions interactions;

    InteractedScript npcInteraction;

    private void Awake()
    {

        interactions = new Interactions();
        input = new PlayerInput();
        
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

            }

        }
        else
        {
            interactions.interact = false;
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

        if(interactions.interact)
        {

            transform.localScale = new Vector3(1.2f, 1.2f, 1);

        }
        else
        {

            transform.localScale = new Vector3(1f, 1, 1);

        }

        Debug.Log(interactions.canInteract);
        
    }

}
class Interactions
{

    public bool interact;
    public bool canInteract;

}
