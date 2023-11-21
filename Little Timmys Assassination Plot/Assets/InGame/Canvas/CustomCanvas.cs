using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCanvas : MonoBehaviour
{

    GameObject referenceCamera;

    Canvas canvas;

    private void Awake()
    {

        canvas = GetComponent<Canvas>();

        referenceCamera = Camera.main.gameObject;

        canvas.worldCamera = referenceCamera.GetComponent<Camera>();

    }

}
