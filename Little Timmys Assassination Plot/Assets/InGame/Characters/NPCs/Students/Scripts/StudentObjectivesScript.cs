using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StudentObjectivesScript : MonoBehaviour
{


    private void Awake()
    {
    }


    #region Objectives

    public void RunToTeacher()
    {

        GetComponent<NavMeshScript>().target.position = new Vector3();

        if(GetComponent<NavMeshScript>().targetReached)
        {


        }

    }

    #endregion

}
