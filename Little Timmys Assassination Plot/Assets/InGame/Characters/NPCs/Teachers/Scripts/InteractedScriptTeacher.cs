using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractedScriptTeacher : MonoBehaviour
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

}
