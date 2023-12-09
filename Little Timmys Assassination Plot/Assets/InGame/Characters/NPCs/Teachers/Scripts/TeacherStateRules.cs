using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherStateRules : MonoBehaviour
{

    public StateModifiers stateModifiers;

    public State state;

    // Start is called before the first frame update
    void Awake()
    {

        stateModifiers = new StateModifiers();

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

    public enum State
    {

        Normal,
        Panic,
        Scared

    }

}
public class StateModifiers
{

    public float speedMultiplier;

    public StateModifiers()
    {

        speedMultiplier = 1;

    }

}
