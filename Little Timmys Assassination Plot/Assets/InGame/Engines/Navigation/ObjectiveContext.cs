using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveContext
{

    public static List<int> ids = new List<int>();
    public int id;

    public GameObject[] entities;
    public State state;
    public float time = 0;

    public enum State
    {
        One,
        Two,
        Three,
        Four,
        Five
    }

    public ObjectiveContext(GameObject[] entities, int id)
    {

        state = State.One;

        this.entities = entities;
        this.id = id;
        

        ids.Add(id);
    }

    public ObjectiveContext(GameObject[] entities, State state, int id)
    {

        this.entities = entities;
        this.state = state;
        this.id = id;

        ids.Add(id);
    }

    public static int GenerateID()
    {

        return (ids.Count + 1);

    }

}
