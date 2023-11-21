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
    [BurstCompile]
    void Awake()
    {
        
        timmyAnimationFrame = 0;
        studentAnimationFrame = 0;
        worldAnimationFrame = 0;

        timmyAnimationTime = 0;
        studentAnimationTime = 0;
        worldAnimationTime = 0;

    }
    [BurstCompile]
    void Update()
    {

        Timers();

        UpdateAnimations();

        fpsUpdate++;

    }
    [BurstCompile]
    void Timers()
    {

        timmyAnimationTime += Time.deltaTime;
        studentAnimationTime += Time.deltaTime;
        worldAnimationTime += Time.deltaTime;
        fpstime += Time.deltaTime;

    }
    [BurstCompile]
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
    [BurstCompile]
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
    [BurstCompile]
    public void ResetTimmyAnimation()
    {

        timmyAnimationTime = 0;
        timmyAnimationFrame = 0;

    }

}
