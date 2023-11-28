using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentStateRules : MonoBehaviour
{

    public StudentStateModifiers stateModifiers;

    State state;

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

                stateModifiers.speedMultiplier = 1.5f;

                break;
            case State.Panic:

                stateModifiers.speedMultiplier = 2;

                break;

        }

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
