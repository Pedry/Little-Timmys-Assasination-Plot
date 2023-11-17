using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class SaveEngineScript : MonoBehaviour
{
    
    private List<GameObject> allObjects;

    private void OnApplicationQuit()
    {
        
        SaveData();
        
    }

    private void SaveData()
    {

        List<GameObject> savableObjects;
        savableObjects = new List<GameObject>();
        
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

            string data = JsonUtility.ToJson(instance);
            
            File.WriteAllText(Application.persistentDataPath + "/MassSave/A" + i, data);
            
            Debug.Log("Object Saved!");

            i++;

        }

    }

}
