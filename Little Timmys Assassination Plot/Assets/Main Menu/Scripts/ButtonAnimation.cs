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
        state = State.Idle;
        spriteLibrary = GetComponent<SpriteLibrary>();
        image = GetComponent<Image>();
    }
    void Update()
    {

        switch (state)
        {
            case State.Idle:
                image.sprite = spriteLibrary.GetSprite("Menu Button", state.ToString());
                break;
            case State.Hover:
                image.sprite = spriteLibrary.GetSprite("Menu Button", state.ToString());
                break;
            case State.Click:

                break;
        }

    }
    public void MouseEnter()
    {
        state = State.Hover;
    }

    public void MouseExit()
    {
        state = State.Idle;
    }

    

}
