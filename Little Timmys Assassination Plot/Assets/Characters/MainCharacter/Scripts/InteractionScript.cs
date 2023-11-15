using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionScript : MonoBehaviour
{

    PlayerInput input;

    Interactions interactions;

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
        }
        else
        {
            interactions.interact = false;
        }

    }

    private void FixedUpdate()
    {
        
    }

}
class Interactions
{

    public bool interact;

}
