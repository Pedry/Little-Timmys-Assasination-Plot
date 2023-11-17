using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowNavMeshCircle : MonoBehaviour
{


    /*
    två postioner 
    hitta postionerna på båda.
    deklarera vilken postion som är spelaren och vilken som är ai.
    få ai att följa efter spelarens positions.
     */



    public GameObject ai;
    public GameObject followPoint;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void FollowStudent()
    {
        Vector3 aiPosition = transform.position;
        Vector3 followPointPosition = followPoint.transform.position;

        aiPosition.y = followPointPosition.y;
        aiPosition.x = followPointPosition.x;

        ai.transform.position = aiPosition;

        
    }

}

