using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoldablesScript : MonoBehaviour
{

    PlayerInput input;

    bool pickingUp = false;

    GameObject heldItem;

    List<GameObject> collidingStudents;

    // Start is called before the first frame update
    void Awake()
    {
        
        collidingStudents = new List<GameObject>();

        input = new PlayerInput();

        heldItem = null;

    }

    private void OnEnable()
    {

        input.Enable();
        input.InGame.PickUp.performed += PickUpInput;

    }

    void PickUpInput(InputAction.CallbackContext context)
    {

        if(heldItem != null)
        {

            if(collidingStudents.Count > 0)
            {

                foreach(GameObject item in collidingStudents)
                {

                    item.GetComponentInChildren<StudentAnimation>().lifeState = StudentAnimation.LifeState.Dead;

                }

                return;

            }

        }

        if(context.ReadValue<float>() == 1)
        {


            pickingUp = true;

        }
        else
        {

            pickingUp = false;

        }

        Debug.Log(pickingUp);

    }

    // Update is called once per frame
    void Update()
    {

        DropItem();
        HoldItem();

    }

    void HoldItem()
    {

        if(heldItem != null)
        {

            heldItem.transform.position = new Vector3(transform.position.x, transform.position.y + 25, heldItem.transform.position.z);

        }

    }

    void DropItem()
    {

        if (pickingUp && heldItem != null)
        {

            heldItem = null;

        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (pickingUp && collision.gameObject.name.Contains("Crowbar"))
        {

            heldItem = collision.gameObject;
            Debug.Log("Picked up!");

            pickingUp = false;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.name.Contains("Body"))
        {

            collidingStudents.Add(collision.gameObject.transform.parent.gameObject);

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.name.Contains("Body"))
        {

            collidingStudents.Remove(collision.gameObject.transform.parent.gameObject);

        }

    }

}
