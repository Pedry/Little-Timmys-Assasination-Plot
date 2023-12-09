using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class SubItemSlot : MonoBehaviour
{

    [SerializeField]
    private int slotIndex;

    HoldablesScript script;

    Image image;
    SpriteLibrary library;

    private void Awake()
    {

        script = GameObject.Find("Timmy").GetComponent<HoldablesScript>();

        image = GetComponent<Image>();
        library = GetComponent<SpriteLibrary>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (script.heldSubItems[slotIndex] != null)
        {
            string itemName = "";

            for (int i = 0; i < script.heldSubItems[slotIndex].name.Length; i++)
            {
                if (script.heldSubItems[slotIndex].name.Substring(i, 1).Equals("(") || script.heldSubItems[slotIndex].name.Substring(i, 1).Equals(" "))
                {
                    break;
                }

                itemName += script.heldSubItems[slotIndex].name.Substring(i, 1);

            }

            image.sprite = library.GetSprite(itemName, itemName);
            image.color = Color.white;

        }
        else
        {

            image.color = Color.clear;

        }

    }
}
