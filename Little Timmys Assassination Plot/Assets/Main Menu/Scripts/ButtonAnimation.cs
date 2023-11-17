using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class ButtonAnimation : Button
{
    new Image image;
    SpriteLibrary spriteLibrary;

    public enum State
    {
        Idle,
        Hover,
        Click
    }

    public State state;

    protected override void Awake()
    {
        base.Awake();
        Cursor.lockState = CursorLockMode.Confined;
        state = State.Idle;
        spriteLibrary = GetComponent<SpriteLibrary>();
        image = GetComponent<Image>();
    }
    void Update()
    {

    }

    

}
