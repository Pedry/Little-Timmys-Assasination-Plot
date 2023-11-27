using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.InputSystem;

public class StudentSaver : MonoBehaviour
{

    StudentAnimation studentAnimation;
    StudentData studentData;

    PlayerInput input;

    string path;

    // Start is called before the first frame update
    void Awake()
    {

        input = new PlayerInput();

        studentAnimation = GetComponent<StudentAnimation>();
        studentData = GetComponent<StudentData>();

        path = Application.persistentDataPath + "/" + studentData.information.name + ".json";

    }

    private void Start()
    {

        LoadData();

    }

    private void OnEnable()
    {

        input.Enable();

        input.InGame.WalkDown.performed += SaveData;


    }

    public void SaveData(InputAction.CallbackContext context)
    {

        Debug.Log("lol");

        studentData.information.pos[0] = transform.position.x;
        studentData.information.pos[1] = transform.position.y;
        studentData.information.pos[2] = transform.position.z;


        if (File.Exists(path))
        {

            Debug.Log("lol");

            File.Delete(path);
            FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(studentData.information, Formatting.Indented));

        }
        else
        {

            Debug.Log("lol2");

            FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(studentData.information, Formatting.Indented));

        }


    }

    public void LoadData()
    {


        if (File.Exists(path))
        {

            studentData.information = JsonConvert.DeserializeObject<PersonalInformation>(File.ReadAllText(path));

            if(studentData.information.pos.Length > 0)
            {

                transform.position = new Vector3(studentData.information.pos[0], studentData.information.pos[1], studentData.information.pos[2]);

            }

        }
        else
        {



        }

    }

}

