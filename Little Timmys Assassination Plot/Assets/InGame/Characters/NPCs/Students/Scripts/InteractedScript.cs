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

    public void SetOutputField(GameObject outputField)
    {

        GetComponent<StudentData>().SetOutputField(outputField);

    }

    public void AskedAbout(string name)
    {

        GetComponent<StudentData>().AskedAbout(name);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


    }

}

class NpcInteractions
{

    public bool playerInteracted;
    public bool NpcInteracted;

}
