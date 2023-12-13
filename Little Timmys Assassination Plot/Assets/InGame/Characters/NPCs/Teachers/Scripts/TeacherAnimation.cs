using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;

public class TeacherAnimation : MonoBehaviour
{

    SpriteLibrary spriteLibrary;
    SpriteRenderer spriteRenderer;

    TeacherData studentData;

    GameObject engine;

    bool resetAnimation;
    bool flippedRight;

    public bool isMoving;

    Vector3 lastPosition;

    [BurstCompile]
    void Awake()
    {

        lastPosition = new Vector3(0, 0, 0);

        isMoving = false;

        flippedRight = false;

        resetAnimation = false;
        
        spriteLibrary = GetComponent<SpriteLibrary>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        studentData = GetComponent<TeacherData>();

        engine = null;

        state = AnimationState.Down;

        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {

            if (obj.tag.Equals("Engine"))
            {

                engine = obj;

            }

        }

    }

    [BurstCompile]
    private void Start()
    {

        spriteRenderer.sprite = spriteLibrary.GetSprite(studentData.information.name, studentData.information.name + "_0");

    }


    [BurstCompile]
    void Update()
    {

        UpdateSprite();

    }

    private void LateUpdate()
    {

        lastPosition = transform.position;

    }

    private void FixedUpdate()
    {
       

    }

    public enum AnimationState
    {

        Down,
        Left,
        Right,
        Up

    }

    public AnimationState state;

    public int lastFrameOffset = 0;
    int lastAnimationFrame;

    [BurstCompile]
    void UpdateSprite()
    {
        string name = studentData.information.name;

        if (!isMoving)
        {

            spriteRenderer.sprite = spriteLibrary.GetSprite(name, name + "_" + (lastFrameOffset));
            return;

        }


        if (state == AnimationState.Left)
        {

            spriteRenderer.flipX = false;

            spriteRenderer.sprite = spriteLibrary.GetSprite(name, name + "_" + (engine.GetComponent<AnimationEngineScript>().studentAnimationFrame + 4));
            lastFrameOffset = 4;
            lastAnimationFrame = engine.GetComponent<AnimationEngineScript>().studentAnimationFrame;
            return;

        }
        if (state == AnimationState.Up)
        {

            spriteRenderer.sprite = spriteLibrary.GetSprite(name, name + "_" + (engine.GetComponent<AnimationEngineScript>().studentAnimationFrame + 8));
            lastFrameOffset = 8;
            lastAnimationFrame = engine.GetComponent<AnimationEngineScript>().studentAnimationFrame;
            return;

        }

        if (state == AnimationState.Down)
        {

            spriteRenderer.sprite = spriteLibrary.GetSprite(name, name + "_" + engine.GetComponent<AnimationEngineScript>().studentAnimationFrame);
            lastFrameOffset = 0;
            lastAnimationFrame = engine.GetComponent<AnimationEngineScript>().studentAnimationFrame;
            return;

        }
        if (state == AnimationState.Right)
        {

            spriteRenderer.flipX = true;


            spriteRenderer.sprite = spriteLibrary.GetSprite(name, name + "_" + (engine.GetComponent<AnimationEngineScript>().studentAnimationFrame + 4));
            lastFrameOffset = 4;
            lastAnimationFrame = engine.GetComponent<AnimationEngineScript>().studentAnimationFrame;
            return;

        }
    }

}
