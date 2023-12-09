using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerRules : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision != null)
        {

            if (collision.gameObject != null)
            {

                if (collision.gameObject.transform.parent != null)
                {

                    if (collision.gameObject.transform.parent.name.Equals("Furniture") && GetComponent<StudentAnimation>().lifeState == StudentAnimation.LifeState.Dead)
                    {

                        GetComponent<SpriteRenderer>().sortingOrder = collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1;

                    }

                }

            }

        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision != null)
        {

            if(collision.gameObject != null)
            {

                if(collision.gameObject.transform.parent != null)
                {

                    if (collision.gameObject.transform.parent.name.Equals("Furniture") && GetComponent<StudentAnimation>().lifeState == StudentAnimation.LifeState.Dead)
                    {

                        GetComponent<SpriteRenderer>().sortingOrder = collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1;

                    }

                }

            }

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision != null)
        {

            if (collision.gameObject != null)
            {

                if (collision.gameObject.transform.parent != null)
                {

                    if (collision.gameObject.transform.parent.name.Equals("Furniture") && GetComponent<StudentAnimation>().lifeState == StudentAnimation.LifeState.Dead)
                    {

                        GetComponent<SpriteRenderer>().sortingOrder = collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder;

                    }

                }

            }

        }

    }

}
