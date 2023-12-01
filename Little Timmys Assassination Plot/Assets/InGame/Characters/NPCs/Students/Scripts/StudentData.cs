using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;

public class StudentData : MonoBehaviour
{

    [SerializeField]
    GameObject mainCharacter;

    [SerializeField]
    public PersonalInformation information;

    GameObject outputField;


    public Dictionary<GameObject, Relation> acquaintances;


    private void Awake()
    {

        information.lastFrame = 0;
        information.animationState = StudentAnimation.AnimationState.Down;

        acquaintances = new Dictionary<GameObject, Relation>();

        int index = 0;

        foreach (GameObject instance in GameObject.FindGameObjectsWithTag("Student"))
        {

            if (instance.GetComponent<TeacherAnimation>() == null)
            {

                acquaintances.Add(instance, new Relation());

            }

        }

    }

    void Start()
    {

    }

    public void SetOutputField(GameObject outputField)
    {

        this.outputField = outputField;

    }

    public void AskedAbout(string name)
    {

        bool hasNoInformation = true;

        foreach (KeyValuePair<GameObject, Relation> relation in acquaintances)
        {

            if (relation.Key.GetComponent<StudentData>().information.name.Equals(name))
            {

                hasNoInformation = false;

                string output;
                string ownerName = information.name;
                string targetName = relation.Key.GetComponent<StudentData>().information.name;
                string targetPronoun1 = "";
                string targetPronoun2 = "";

                switch (relation.Key.GetComponent<StudentData>().information.gender)
                {

                    case PersonalInformation.Gender.Male:
                        targetPronoun1 = "him";
                        targetPronoun2 = "he";
                        break;
                    case PersonalInformation.Gender.Female:
                        targetPronoun1 = "her";
                        targetPronoun2 = "she";
                        break;

                }

                string Message = "";

                switch (relation.Value.likes)
                {
                    case -3:
                        Message = "I HATE " + targetPronoun1;
                        break;
                    case -2:
                        Message = "I Despice " + targetPronoun1;
                        break;
                    case -1:
                        Message = "I don't like " + targetPronoun1;
                        break;
                    case 0:
                        Message = "I don't really know " + targetPronoun1;
                        break;
                    case 1:
                        Message = "I think " + targetPronoun2 + " nice?";
                        break;
                    case 2:
                        Message = "Yeah " + targetPronoun2 + " is my friend!";
                        break;
                    case 3:
                        Message = targetName + " is my best friend!";
                        break;
                }

                

                output = "\n" + ownerName + ": " + Message;
                outputField.GetComponent<TextMeshProUGUI>().SetText(output);

            }

        }

        if(hasNoInformation)
        {

            outputField.GetComponent<TextMeshProUGUI>().SetText(information.name + ": I don't know that person..");

        }

    }
}

[Serializable]
public class PersonalInformation
{
    public enum Gender
    {
        Male,
        Female
    }

    public enum FriendGroup
    {

        Group1,
        Group2,
        Group3,
        Group4,
        Group5,
        Group6,
        Group7,
        Group8

    }

    [SerializeField]
    public KeyValuePair<string, Relation>[] pairs;

    public StudentAnimation.LifeState lifeState;
    public float[] savePosition;

    public float[] saveNavPointPosition;

    public FriendGroup friendGroup;
    public Gender gender;
    public string name;

    public PersonalInformation()
    {

        savePosition = new float[3];
        saveNavPointPosition = new float[3];

    }

    public int lastFrame;
    public StudentAnimation.AnimationState animationState;


}

[Serializable]
public struct Relation
{

    public string lastSeen;
    public string opinion;
    public int likes;

}
