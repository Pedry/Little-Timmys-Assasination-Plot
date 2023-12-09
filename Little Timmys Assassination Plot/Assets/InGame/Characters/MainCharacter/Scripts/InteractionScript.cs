using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
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
        input.InGame.DragHuman.performed += OnDragHuman;

    }

    private void OnDisable()
    {
        
        input.Disable();

    }

    void OnInteract(InputAction.CallbackContext context)
    {

        if(context.ReadValue<float>() == 1)
        {

            if(collidingSofa != null)
            {

                if (collidingSofa.GetComponent<SofaCoin>().HasCoin())
                {

                    collidingSofa.GetComponent<SofaCoin>().GetCoin();

                    Debug.Log(collidingSofa.name);

                }

            }


            if (npcInteraction != null)
            {


                if (npcInteraction.GetComponent<StudentAnimation>().isMoving)
                {

                    return;

                }

            }

            interactions.interact = true;

            if(npcInteraction != null)
            {

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

    void CollectCoin()
    {



    }

    void OnDragHuman(InputAction.CallbackContext contex)
    {

        if(contex.ReadValue<float>() == 1)
        {

            HandleDragingHuman();

        }

    }

    void HandleDragingHuman()
    {

        if (GetComponent<HoldablesScript>().dragingHuman != null)
        {

            GetComponent<HoldablesScript>().dragingHuman.GetComponent<NavMeshAgent>().Warp(GetComponent<HoldablesScript>().dragingHuman.transform.position);
            GetComponent<HoldablesScript>().dragingHuman.GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<HoldablesScript>().dragingHuman = null;

            return;

        }
        else
        {

            if(npcInteraction != null)
            {

                if (npcInteraction.gameObject.layer == LayerMask.NameToLayer("Corpses"))
                {

                    GetComponent<HoldablesScript>().dragingHuman = npcInteraction.gameObject;
                    GetComponent<HoldablesScript>().lastHeldHumanPosition = npcInteraction.gameObject.transform.position;

                }

            }

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

    GameObject collidingSofa;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.gameObject.name.Contains("Sofa"))
        {

            collidingSofa = collision.collider.gameObject;

        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.collider.gameObject.name.Contains("Sofa"))
        {

            collidingSofa = collision.collider.gameObject;

        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.collider.gameObject.name.Contains("Sofa"))
        {

            collidingSofa = null;

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
