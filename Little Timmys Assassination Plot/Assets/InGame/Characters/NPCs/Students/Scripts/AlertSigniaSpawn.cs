using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class AlertSigniaSpawn : MonoBehaviour
{

    SpriteLibrary spriteLibrary;
    SpriteRenderer spriteRenderer;

    float timestep;

    float animationFrame;

    float frameCount;

    private void Awake()
    {
        spriteLibrary = GetComponent<SpriteLibrary>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

        frameCount = 0;
        timestep = 0;
        animationFrame = 0;

    }

    // Update is called once per frame
    void Update()
    {

        spriteRenderer.sprite = spriteLibrary.GetSprite("Exclamation", "Exclamation_" + animationFrame.ToString());

        timestep += Time.deltaTime;

        if(timestep > 0.34/(9*2))
        {

            timestep = 0;
            animationFrame++;
            frameCount++;

        }

        if (spriteLibrary.GetSprite("Exclamation", "Exclamation_" + animationFrame.ToString()) == null)
        {

            animationFrame = 0;

        }

        if(frameCount > 9*2)
        {

            Destroy(gameObject);

        }

    }

}
