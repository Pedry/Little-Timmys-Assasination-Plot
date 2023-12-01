using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectSaver : MonoBehaviour
{

    [SerializeField]
    Data data = new Data();

    // Start is called before the first frame update
    void Awake()
    {

        data.position = new float[3];
        data.path = Application.persistentDataPath + "/" + name + ".ltap";
    }

    public void SaveData()
    {

        SavePosition();

        if (File.Exists(data.path))
        {

            File.Delete(data.path);
            FileStream stream = File.Create(data.path);
            stream.Close();
            File.WriteAllText(data.path, SavingEngineScript.Encrypt(JsonConvert.SerializeObject(data, Formatting.Indented)));

        }
        else
        {

            FileStream stream = File.Create(data.path);
            stream.Close();
            File.WriteAllText(data.path, SavingEngineScript.Encrypt(JsonConvert.SerializeObject(data, Formatting.Indented)));

        }


    }

    public void LoadData()
    {

        if (File.Exists(data.path))
        {

            data = JsonConvert.DeserializeObject<Data>(SavingEngineScript.Decrypt(File.ReadAllText(data.path)));

            LoadPosition();

        }

    }

    void SavePosition()
    {

        data.position[0] = transform.position.x;
        data.position[1] = transform.position.y;
        data.position[2] = transform.position.z;

    }
    void LoadPosition()
    {

        transform.position = new Vector3(
            data.position[0],
            data.position[1],
            data.position[2]);

    }

    [Serializable]
    struct Data
    {

        public float[] position;
        public string path;

    }
}
