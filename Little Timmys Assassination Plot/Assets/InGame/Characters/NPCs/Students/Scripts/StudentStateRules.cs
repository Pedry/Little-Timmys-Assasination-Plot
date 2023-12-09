using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentStateRules : MonoBehaviour
{

    public StudentStateModifiers stateModifiers;

    public State state;

    // Start is called before the first frame update
    void Awake()
    {

        stateModifiers = new StudentStateModifiers();

        state = State.Normal;

    }

    // Update is called once per frame
    void Update()
    {
        
        switch (state)
        {

            case State.Normal:

                stateModifiers.speedMultiplier = 1;

                break;
            case State.Scared:

                stateModifiers.speedMultiplier = 1.7f;

                break;
            case State.Panic:

                stateModifiers.speedMultiplier = 2.3f;

                break;

        }

    }

    public void UpdateCollider()
    {

        BoxCollider2D newCollider = GetComponent<BoxCollider2D>();
        newCollider.offset = new Vector2(0, -11);
        newCollider.size = new Vector2(48, 29);

    }

    public enum State
    {

        Normal,
        Panic,
        Scared

    }

}
public class StudentStateModifiers
{

    public float speedMultiplier;

    public StudentStateModifiers()
    {

        speedMultiplier = 1;

    }

}
