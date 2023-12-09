using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FurnitureSaver : MonoBehaviour
{

    [SerializeField]
    Data data = new Data();

    // Start is called before the first frame update
    void Awake()
    {

        data.path = Application.persistentDataPath + "/" + name + ".ltap";
        data.thingExists = false;

    }

    public void SaveData()
    {

        if (File.Exists(data.path))
        {

            File.Delete(data.path);
            FileStream stream = File.Create(data.path);
            stream.Close();
            File.WriteAllText(data.path, (JsonConvert.SerializeObject(data, Formatting.Indented)));

        }
        else
        {

            FileStream stream = File.Create(data.path);
            stream.Close();
            File.WriteAllText(data.path, (JsonConvert.SerializeObject(data, Formatting.Indented)));

        }


    }

    public void LoadData()
    {

        if (File.Exists(data.path))
        {

            data = JsonConvert.DeserializeObject<Data>((File.ReadAllText(data.path)));

            if(GetComponent<SofaCoin>() != null)
            {

                GetComponent<SofaCoin>().hasCoin = data.thingExists;

            }

        }

    }

    public void SaveStats(bool instance)
    {

        data.thingExists = instance;

    }

    [Serializable]
    struct Data
    {

        public string path;
        public bool thingExists;

    }

}
