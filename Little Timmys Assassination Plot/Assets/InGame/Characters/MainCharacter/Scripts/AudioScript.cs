using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMODUnityResonance;
using FMOD.Studio;

public class AudioScript : MonoBehaviour
{

    [SerializeField]
    EventReference eventReference;

    EventInstance eventInstance;

    oMeter oMeter;

    // Start is called before the first frame update
    void Start()
    {

        oMeter = GameObject.Find("oMeter").GetComponent<oMeter>();

        eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstance.start();

    }

    // Update is called once per frame
    void Update()
    {

        eventInstance.setParameterByName("Sinnestillstånd", oMeter.angervalue / 3);
        
    }
}
