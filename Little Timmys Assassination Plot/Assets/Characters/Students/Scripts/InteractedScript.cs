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

    public void ChangeColor()
    {

        spriteRenderer.color = new Color(Mathf.Clamp01(spriteRenderer.color.r + Random.Range(-0.2f, 0.2f)), Mathf.Clamp01(spriteRenderer.color.g + Random.Range(-0.2f, 0.2f)), Mathf.Clamp01(spriteRenderer.color.b + Random.Range(-0.2f, 0.2f)), 1);

    }
}

class NpcInteractions
{

    public bool playerInteracted;
    public bool NpcInteracted;

}
