using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class StudentObjectivesScript : MonoBehaviour
{

    public delegate bool Objective(ObjectiveContext context);
    public Dictionary<ObjectiveContext, Objective> objectives;

    private void Awake()
    {

        objectives = new Dictionary<ObjectiveContext, Objective>();


    }

    private void FixedUpdate()
    {

        if(objectives.Count > 0)
        {

            DoNextObjectiveUntilCompleted(objectives.ElementAt<KeyValuePair<ObjectiveContext, Objective>>(0));

        }

    }

    void DoNextObjectiveUntilCompleted(KeyValuePair<ObjectiveContext, Objective> currentObjective)
    {

        bool completed = currentObjective.Value(currentObjective.Key);

        if(completed)
        {

            objectives.Remove(currentObjective.Key);

        }


    }

}
