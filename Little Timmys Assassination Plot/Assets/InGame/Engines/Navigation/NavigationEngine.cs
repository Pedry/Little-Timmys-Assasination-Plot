using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationEngine : MonoBehaviour
{

    public static List<GameObject> navigationTiles;

    // Start is called before the first frame update
    void Awake()
    {
        navigationTiles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
