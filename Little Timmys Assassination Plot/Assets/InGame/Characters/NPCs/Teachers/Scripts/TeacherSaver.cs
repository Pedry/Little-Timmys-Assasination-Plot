using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

public class TeacherSaver : MonoBehaviour
{

    Data data;

    private void Awake()
    {
        
        data = new Data();
        data.path = Application.persistentDataPath + "/" + GetComponent<TeacherData>().information.name + ".ltap";
        data.position = new float[3];
        data.navPointPosition = new float[3];
        data.lastFrame = 0;
        data.animationState = TeacherAnimation.AnimationState.Down;

    }

    public void SaveData()
    {

        data.position[0] = transform.position.x;
        data.position[1] = transform.position.y;
        data.position[2] = transform.position.z;

        data.navPointPosition[0] = GetComponent<NavMeshScriptTeacher>().target.position.x;
        data.navPointPosition[1] = GetComponent<NavMeshScriptTeacher>().target.position.y;
        data.navPointPosition[2] = GetComponent<NavMeshScriptTeacher>().target.position.z;

        data.lastFrame = GetComponent<TeacherAnimation>().lastFrameOffset;
        data.animationState = GetComponent<TeacherAnimation>().state;

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

            GetComponent<NavMeshScriptTeacher>().agent.Warp(new Vector3(
                data.position[0],
                data.position[1],
                data.position[2]));

            transform.position = new Vector3(
                data.position[0],
                data.position[1],
                data.position[2]);

            GetComponent<NavMeshScriptTeacher>().target.position = new Vector3(
                data.navPointPosition[0],
                data.navPointPosition[1],
                data.navPointPosition[2]);

            GetComponent<TeacherData>().information.savePosition = data.position;
            GetComponent<TeacherData>().information.saveNavPointPosition = data.navPointPosition;

            GetComponent<TeacherAnimation>().lastFrameOffset = data.lastFrame;
            GetComponent<TeacherAnimation>().state = data.animationState;


        }
        else
        {

            GetComponent<NavMeshScriptTeacher>().RandomizeNavigation();

        }

    }

    struct Data
    {

        public string path;
        public float[] position;
        public float[] navPointPosition;
        public int lastFrame;
        public TeacherAnimation.AnimationState animationState;


    }
}
