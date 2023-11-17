using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class SaveEngineScript : MonoBehaviour
{
    
    private List<GameObject> allObjects;

    [SerializeField]
    List<GameObject> savableObjects;

    private void OnApplicationQuit()
    {
        
        SaveData();
        
    }

    private void SaveData()
    {

        allObjects = FindObjectsOfType<GameObject>().ToList();

        foreach (GameObject instance in allObjects)
        {

            if (instance.GetComponent<ISavable>() != null)
            {

                savableObjects.Add(instance);

            }

        }

        int i = 1;

        foreach (GameObject instance in savableObjects)
        {

            instance.GetComponent<ISavable>().SaveData();

        }

    }

}
