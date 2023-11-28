using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractedScript : MonoBehaviour
{

    SpriteRenderer spriteRenderer;

    NpcInteractions npcInteractions;

    private void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        
        npcInteractions = new NpcInteractions();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {



    }

    public void SetOutputField(GameObject outputField)
    {

        GetComponent<StudentData>().SetOutputField(outputField);

    }

    public void AskedAbout(string name)
    {

        GetComponent<StudentData>().AskedAbout(name);

    }

    void RaycastVision()
    {

        List<GameObject> list = new List<GameObject>();

        List<Collider2D> collision = new List<Collider2D>();


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log(collision.gameObject.name);

    }

}

class NpcInteractions
{

    public bool playerInteracted;
    public bool NpcInteracted;

}
