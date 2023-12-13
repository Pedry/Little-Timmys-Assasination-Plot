using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class oMeter : MonoBehaviour
{

    public float angervalue;

    public List<GameObject> corpses = new List<GameObject>();

    int animationFrame;
    int angerIndex;

    float time;

    SpriteLibrary library;
    Image image;

    GameObject timmy;
    GameObject biff;

    void Awake()
    {
        angerIndex = 0;
        time = 0;

        library = GetComponent<SpriteLibrary>();
        image = GetComponent<Image>();

        timmy = GameObject.Find("Timmy");
        biff = GameObject.Find("Biff").GetComponentInChildren<StudentData>().gameObject;

    }

    // Update is called once per frame
    void Update()
    {

        TimmyAnger();

        ClampAnger();

        time += Time.deltaTime;

        if(time > 0.12f)
        {

            if(library.GetSprite("oMeter" + angerIndex, "oMeter" + angerIndex + "_" + animationFrame) == null)
            {

                animationFrame = 0;

            }

            image.sprite = library.GetSprite("oMeter" + angerIndex, "oMeter" + angerIndex + "_" + animationFrame);

            time = 0;
            animationFrame++;

        }


    }

    void ClampAnger()
    {


        angervalue = Mathf.Clamp(angervalue, 0, 9.9f);

        angerIndex = (int) Mathf.Floor(angervalue);

    }

    float angerEffect = 0.2f;

    void TimmyAnger()
    {

        bool noAngereffect1 = false;
        bool noAngereffect2 = false;

        Vector2 timmyPosition = timmy.transform.position;
        Vector2 biffPosition = biff.transform.position;

        Vector2 positionDelta = timmyPosition - biffPosition;

        if(positionDelta.magnitude < 30)
        {

            angervalue += Time.deltaTime * 12 * angerEffect;

        }
        else if (positionDelta.magnitude < 60)
        {

            angervalue += Time.deltaTime * 10 * angerEffect;

        }
        else if (positionDelta.magnitude < 90)
        {

            angervalue += Time.deltaTime * 5 * angerEffect;

        }
        else if (positionDelta.magnitude < 150)
        {

            angervalue += Time.deltaTime * 2 * angerEffect;

        }
        else
        {

            noAngereffect1 = true;

        }

        foreach(GameObject corpse in corpses)
        {

            Vector2 corpsePosition = corpse.transform.position;

            positionDelta = timmyPosition - corpsePosition;

            if (positionDelta.magnitude < 30)
            {

                angervalue += Time.deltaTime * 12 * angerEffect;
                noAngereffect2 = false;

            }
            else if (positionDelta.magnitude < 60)
            {

                angervalue += Time.deltaTime * 10 * angerEffect;
                noAngereffect2 = false;

            }
            else if (positionDelta.magnitude < 90)
            {

                angervalue += Time.deltaTime * 5 * angerEffect;
                noAngereffect2 = false;

            }
            else if (positionDelta.magnitude < 150)
            {

                angervalue += Time.deltaTime * 2 * angerEffect;
                noAngereffect2 = false;

            }
            else
            {

                noAngereffect2 = true;

            }

        }

        if(noAngereffect1 && noAngereffect2)
        {

            angervalue -= Time.deltaTime * 1.5f * angerEffect;

        }

    }

}
