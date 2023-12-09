using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class MainItemSlot : MonoBehaviour
{

    GameObject item;

    Image image;
    SpriteLibrary library;

    private void Awake()
    {
        item = null;

        image = GetComponent<Image>();
        library = GetComponent<SpriteLibrary>();

    }

    // Update is called once per frame
    void Update()
    {

        item = GameObject.Find("Timmy").GetComponent<HoldablesScript>().heldItem;


        if(item != null)
        {

            string itemName = "";

            for(int i = 0; i < item.name.Length; i++)
            {

                if(item.name.Substring(i, 1).Equals("(") || item.name.Substring(i, 1).Equals(" "))
                {
                    break;
                }

                itemName += item.name.Substring(i, 1);

            }

            image.color = Color.white;
            image.sprite = library.GetSprite(itemName, itemName);

        }
        else
        {

            image.color = Color.clear;

        }

    }
}
