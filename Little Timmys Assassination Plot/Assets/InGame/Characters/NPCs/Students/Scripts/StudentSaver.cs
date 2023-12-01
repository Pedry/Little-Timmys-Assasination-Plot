using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.InputSystem;
using System.Linq;
using System.Threading;

public class StudentSaver : MonoBehaviour
{

    StudentAnimation studentAnimation;
    StudentData studentData;


    string path;


    // Start is called before the first frame update
    void Awake()
    {

        studentAnimation = GetComponent<StudentAnimation>();
        studentData = GetComponent<StudentData>();

        path = Application.persistentDataPath + "/" + studentData.information.name + ".ltap";


    }

    public void SaveData()
    {

        studentData.information.savePosition[0] = transform.position.x;
        studentData.information.savePosition[1] = transform.position.y;
        studentData.information.savePosition[2] = transform.position.z;

        studentData.information.saveNavPointPosition[0] = GetComponent<NavMeshScript>().target.position.x;
        studentData.information.saveNavPointPosition[1] = GetComponent<NavMeshScript>().target.position.y;
        studentData.information.saveNavPointPosition[2] = GetComponent<NavMeshScript>().target.position.z;


        Dictionary<string, Relation> tempPairs = new Dictionary<string, Relation>();

        foreach(KeyValuePair<GameObject, Relation> instance in studentData.acquaintances)
        {

            tempPairs.Add(instance.Key.GetComponent<StudentData>().information.name, instance.Value);

        }

        studentData.information.pairs = tempPairs.ToArray();

        studentData.information.lastFrame = GetComponent<StudentAnimation>().lastFrameOffset;
        studentData.information.animationState = GetComponent<StudentAnimation>().state;

        Thread thread = new Thread(ThreadedSerialisation);
        thread.IsBackground = true;
        thread.Start();

    }

    void ThreadedSerialisation()
    {

        if (File.Exists(path))
        {


            File.Delete(path);
            FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, SavingEngineScript.Encrypt(JsonConvert.SerializeObject(studentData.information, Formatting.Indented)));

        }
        else
        {


            FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, SavingEngineScript.Encrypt(JsonConvert.SerializeObject(studentData.information, Formatting.Indented)));

        }

    }

    public void LoadData()
    {

        if (File.Exists(path))
        {

            studentData.information = JsonConvert.DeserializeObject<PersonalInformation>(SavingEngineScript.Decrypt(File.ReadAllText(path)));

            Dictionary<GameObject, Relation> tempPairs = new Dictionary<GameObject, Relation>();

            foreach (KeyValuePair<string, Relation> instance in studentData.information.pairs)
            {

                tempPairs.Add(GameObject.Find(instance.Key).GetComponentInChildren<StudentData>().gameObject, instance.Value);

            }

            studentData.acquaintances = tempPairs;

            if (studentData.information.savePosition.Length > 0)
            {

                transform.position = new Vector3(
                    studentData.information.savePosition[0], 
                    studentData.information.savePosition[1], 
                    studentData.information.savePosition[2]);

                GetComponent<NavMeshScript>().target.gameObject.transform.position = new Vector3(
                    studentData.information.saveNavPointPosition[0], 
                    studentData.information.saveNavPointPosition[1], 
                    studentData.information.saveNavPointPosition[2]);

                GetComponent<NavMeshScript>().agent.nextPosition = new Vector3(
                    studentData.information.savePosition[0],
                    studentData.information.savePosition[1],
                    studentData.information.savePosition[2]);

                GetComponent<NavMeshScript>().agent.Warp(new Vector3(
                    studentData.information.savePosition[0],
                    studentData.information.savePosition[1],
                    studentData.information.savePosition[2]));

            }

            GetComponent<StudentAnimation>().lastFrameOffset = studentData.information.lastFrame;
            GetComponent<StudentAnimation>().state = studentData.information.animationState;

        }

    }

}

