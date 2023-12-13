using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class AnimationEngineScript : MonoBehaviour
{

    public int timmyAnimationFrame;
    public int studentAnimationFrame;
    public int worldAnimationFrame;
    public int fps;
    public int fpsUpdate;

    float timmyAnimationTime;
    float studentAnimationTime;
    float worldAnimationTime;
    float fpstime;

    [SerializeField]
    float timmyAnimationTimeCap;
    [SerializeField]
    float studentAnimationTimeCap;
    [SerializeField]
    float worldAnimationTimeCap;

    void Awake()
    {
        
        timmyAnimationFrame = 0;
        studentAnimationFrame = 0;
        worldAnimationFrame = 0;

        timmyAnimationTime = 0;
        studentAnimationTime = 0;
        worldAnimationTime = 0;

        Application.targetFrameRate = -1;
        QualitySettings.vSyncCount = 1;

    }

    void Update()
    {

        Timers();

        UpdateAnimations();

        fpsUpdate++;

    }

    void Timers()
    {

        timmyAnimationTime += Time.deltaTime;
        studentAnimationTime += Time.deltaTime;
        worldAnimationTime += Time.deltaTime;
        fpstime += Time.deltaTime;

    }

    void UpdateAnimations()
    {

        if(timmyAnimationTime > timmyAnimationTimeCap)
        {

            timmyAnimationTime = 0;
            timmyAnimationFrame++;

        }

        if(studentAnimationTime > studentAnimationTimeCap)
        {

            studentAnimationTime = 0;
            studentAnimationFrame++;

        }

        if(worldAnimationTime > worldAnimationTimeCap)
        {

            worldAnimationTime = 0;
            worldAnimationFrame++;

        }

        if(fpstime > 1)
        {
            fpstime = 0;
            fps = fpsUpdate;
            fpsUpdate = 0;
        }

        CycleAnimationFrame();

    }

    void CycleAnimationFrame()
    {

        if(timmyAnimationFrame == 4)
        {

            timmyAnimationFrame = 0;

        }

        if(studentAnimationFrame == 4)
        {

            studentAnimationFrame = 0;

        }

        if(worldAnimationFrame == 4)
        {

            worldAnimationFrame = 0;

        }

    }

    public void ResetTimmyAnimation()
    {

        timmyAnimationTime = 0;
        timmyAnimationFrame = 0;

    }

}
