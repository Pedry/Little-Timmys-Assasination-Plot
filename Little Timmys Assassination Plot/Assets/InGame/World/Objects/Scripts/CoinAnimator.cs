using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CoinAnimator : MonoBehaviour
{

    SpriteLibrary library;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Awake()
    {
        library = GetComponent<SpriteLibrary>();
        sprite = GetComponent<SpriteRenderer>();
        animationTime = 0;
        animationFrame = 0;
    }

    float animationTime;
    int animationFrame;

    // Update is called once per frame
    void Update()
    {

        animationTime += Time.deltaTime;

        if (animationTime > 0.12f)
        {

            if(library.GetSprite("Coin", "Coin_" + animationFrame) != null)
            {

                sprite.sprite = library.GetSprite("Coin", "Coin_" + animationFrame);

            }
            else
            {

                animationFrame = 0;
                sprite.sprite = library.GetSprite("Coin", "Coin_" + animationFrame);

            }

            animationFrame++;

        }

    }

}
