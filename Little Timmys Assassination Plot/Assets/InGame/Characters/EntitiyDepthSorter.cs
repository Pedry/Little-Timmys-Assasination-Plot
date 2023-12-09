using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class EntityDepthSorter : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        SpriteSorting();

    }

    void SpriteSorting()
    {

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 100);

    }
}