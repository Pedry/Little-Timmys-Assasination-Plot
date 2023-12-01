using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

public class TimmySaver : MonoBehaviour
{

    [SerializeField]
    public Data data;

    private void Awake()
    {
        data = new Data();
        data.position = new float[3];
        data.lookDirection = "Down";
        data.lastFrameOffset = 0;
        data.path = Application.persistentDataPath + "/Timmy.ltap";
    }

    public void SaveData()
    {


        data.position[0] = transform.position.x;
        data.position[1] = transform.position.y;
        data.position[2] = transform.position.z;

        data.lookDirection = GetComponent<playerController>().lastCategory;
        data.lastFrameOffset = GetComponent<playerController>().lastFrameOffset;



        if (GetComponent<HoldablesScript>().heldItem != null)
        {

            data.heldItem = GetComponent<HoldablesScript>().heldItem.name;

        }
        else
        {

            data.heldItem = null;

        }

        Thread thread = new Thread(ThreadedSerialisation);
        thread.IsBackground = true;
        thread.Start();




    }

    void ThreadedSerialisation()
    {

        if (File.Exists(data.path))
        {

            File.Delete(data.path);
            FileStream stream = File.Create(data.path);
            stream.Close();
            File.WriteAllText(data.path, JsonConvert.SerializeObject(data, Formatting.Indented));

        }
        else
        {

            FileStream stream = File.Create(data.path);
            stream.Close();
            File.WriteAllText(data.path, JsonConvert.SerializeObject(data, Formatting.Indented));

        }

    }

    public void LoadData()
    {

        if (File.Exists(data.path))
        {

            data = JsonConvert.DeserializeObject<Data>(File.ReadAllText(data.path));

            LoadPosition();
            LoadItem();

        }

    }

    void LoadPosition()
    {

        transform.position = new Vector3(
            data.position[0],
            data.position[1],
            data.position[2]);

        GetComponent<playerController>().lastCategory = data.lookDirection;
        GetComponent<playerController>().lastFrameOffset = data.lastFrameOffset;

    }

    void LoadItem()
    {

        if (data.heldItem != null)
        {

            foreach (GameObject obj in FindObjectsOfType<GameObject>())
            {

                if (obj.name.Equals(data.heldItem))
                {

                    GetComponent<HoldablesScript>().heldItem = obj;

                }

            }

        }

    }

    [Serializable]
    public struct Data
    {

        public float[] position;
        public string lookDirection;
        public int lastFrameOffset;
        public string path;
        public string heldItem;


    }

}
