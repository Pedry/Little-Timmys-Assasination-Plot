using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class StudentVisionScript : MonoBehaviour
{

    public Dictionary<GameObject, float> studentsInView = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> teachersInView = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> corpsesInView = new Dictionary<GameObject, float>();
    public GameObject lastSeenTeacher;

    GameObject timmy;


    int visionResolution;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

        visionResolution = 80;
        lastSeenTeacher = GameObject.Find("Lucy").GetComponentInChildren<TeacherData>().gameObject;

    }

    [BurstCompile]
    public struct GeneratePositionsJob : IJobParallelFor
    {
        public int count;
        public NativeArray<Vector2> positions;

        public void Execute(int i)
        {
            float angle = i * (360f / count);
            float radians = Mathf.Deg2Rad * angle;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);

            Vector2 position = new Vector2(x, y);
            positions[i] = position;
        }
    }

    public bool hasVision = true;

    // Update is called once per frame
    void FixedUpdate()
    {

        studentsInView.Clear();
        teachersInView.Clear();
        corpsesInView.Clear();
        timmy = null;

        if (hasVision)
        {

            CheckVision();

        }

    }

    void CheckVision()
    {

        NativeArray<Vector2> positions = new NativeArray<Vector2>(visionResolution, Allocator.TempJob);

        GeneratePositionsJob job = new GeneratePositionsJob
        {
            count = visionResolution,
            positions = positions
        };

        JobHandle jobHandle = job.Schedule(visionResolution, 15); // You can adjust the batch size (64 in this case) based on your requirements

        jobHandle.Complete();

        List<Vector2> positionsList = new List<Vector2>(positions.ToArray());

        positions.Dispose();

        // Use the positionsList as needed

        foreach (Vector2 view in positionsList)
        {


            int layer_mask = LayerMask.GetMask("lol", "Vision", "Corpses");



            RaycastHit2D hit = Physics2D.Raycast(transform.position, view, 190, layer_mask);

            if (hit.collider == null)
            {
                continue;
            }

            if (hit.collider.gameObject.name.Equals("Timmy"))
            {

                timmy = hit.collider.gameObject;

                if (timmy.GetComponent<HoldablesScript>().dragingHuman)
                {

                    SeesTimmyDragHumon();

                    GetComponent<StudentStateRules>().state = StudentStateRules.State.Panic;

                }

            }

            if (hit.collider.gameObject.transform.parent.transform.parent != null)
            {

                if (hit.collider.gameObject.transform.parent.transform.parent.name.Equals("Students"))
                {


                    if (hit.collider.gameObject.GetComponent<StudentData>())
                    {

                        if (!studentsInView.ContainsKey(hit.collider.gameObject))
                        {

                            if (hit.collider.gameObject.GetComponent<SpriteRenderer>().sortingOrder == spriteRenderer.sortingOrder)
                            {

                                if(hit.collider.gameObject.GetComponent<StudentAnimation>().lifeState == StudentAnimation.LifeState.Dead)
                                {

                                    if (!corpsesInView.ContainsKey(hit.collider.gameObject))
                                    {

                                        corpsesInView.Add(hit.collider.gameObject, 0);

                                    }

                                }
                                else
                                {

                                    studentsInView.Add(hit.collider.gameObject, 0);

                                }

                            }

                        }

                    }

                }

            }


            if (hit.collider.gameObject.transform.parent.transform.parent != null)
            {

                if (hit.collider.gameObject.transform.parent.transform.parent.name.Equals("Teachers"))
                {

                    if (hit.collider.gameObject.GetComponent<TeacherData>())
                    {

                        if (!teachersInView.ContainsKey(hit.collider.gameObject))
                        {

                            teachersInView.Add(hit.collider.gameObject, 0);
                            lastSeenTeacher = hit.collider.gameObject;

                        }

                    }

                }

            }

        }

    }

    void SeesTimmyDragHumon()
    {

        if (GetComponent<StudentStateRules>().state != StudentStateRules.State.Panic)
        {
            GameObject[] objectiveEntities = new GameObject[1] { gameObject };
            GameObject allertSignia = Instantiate(GetComponent<StudentData>().alertSignia);
            allertSignia.transform.position = transform.position + new Vector3(0, 40, 0);

            ObjectiveContext jumpUpAndDownBeforeObjective = new ObjectiveContext(objectiveEntities, ObjectiveContext.State.One, ObjectiveContext.GenerateID());
            ObjectiveContext runToTeacherObjective = new ObjectiveContext(objectiveEntities, ObjectiveContext.GenerateID());
            ObjectiveContext jumpUpAndDownAfterObjective = new ObjectiveContext(objectiveEntities, ObjectiveContext.State.One, ObjectiveContext.GenerateID());

            float recordTime = 0;

            GetComponent<StudentObjectivesScript>().objectives.Add(jumpUpAndDownBeforeObjective,
                (jumpUpAndDownBeforeObjective) =>
                {

                    recordTime += Time.deltaTime;

                    if (jumpUpAndDownBeforeObjective.entities[0].GetComponent<NavMeshScript>() != null)
                    {

                        Transform navigationPoint = jumpUpAndDownBeforeObjective.entities[0].GetComponent<NavMeshScript>().target;
                        navigationPoint.position = jumpUpAndDownBeforeObjective.entities[0].transform.position;

                        float jumpTime = 0.07f;
                        float jumpSpeed = 150f;

                        int witnessIndex = 0;
                        int restartTime = 0;

                        int animationFrame;

                        jumpUpAndDownBeforeObjective.time += Time.deltaTime;

                        switch (jumpUpAndDownBeforeObjective.state)
                        {


                            case ObjectiveContext.State.One:

                                jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentAnimation>().animationObjective = true;

                                animationFrame = 1;

                                if (jumpUpAndDownBeforeObjective.time < jumpTime)
                                {

                                    jumpUpAndDownBeforeObjective.entities[0].GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpSpeed;

                                    jumpUpAndDownBeforeObjective.entities[0].GetComponent<SpriteRenderer>().sprite =
                                    jumpUpAndDownBeforeObjective.entities[0].GetComponent<SpriteLibrary>().GetSprite
                                    (

                                        jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentData>().information.name,
                                        jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentData>().information.name + "_" + (jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentAnimation>().lastFrameOffset + animationFrame)

                                    );


                                }
                                else
                                {

                                    jumpUpAndDownBeforeObjective.state = ObjectiveContext.State.Two;
                                    jumpUpAndDownBeforeObjective.time = restartTime;

                                }

                                break;

                            case ObjectiveContext.State.Two:

                                jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentAnimation>().animationObjective = true;

                                animationFrame = 2;

                                if (jumpUpAndDownBeforeObjective.time < jumpTime)
                                {

                                    jumpUpAndDownBeforeObjective.entities[0].GetComponent<Rigidbody2D>().velocity = -Vector2.up * jumpSpeed;

                                    jumpUpAndDownBeforeObjective.entities[0].GetComponent<SpriteRenderer>().sprite =
                                    jumpUpAndDownBeforeObjective.entities[0].GetComponent<SpriteLibrary>().GetSprite
                                    (

                                        jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentData>().information.name,
                                        jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentData>().information.name + "_" + (jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentAnimation>().lastFrameOffset + animationFrame)

                                    );

                                }
                                else
                                {

                                    jumpUpAndDownBeforeObjective.state = ObjectiveContext.State.Three;
                                    jumpUpAndDownBeforeObjective.time = restartTime;

                                }

                                break;

                            case ObjectiveContext.State.Three:

                                jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentAnimation>().animationObjective = true;

                                animationFrame = 3;

                                if (jumpUpAndDownBeforeObjective.time < jumpTime)
                                {

                                    jumpUpAndDownBeforeObjective.entities[0].GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpSpeed;

                                    jumpUpAndDownBeforeObjective.entities[0].GetComponent<SpriteRenderer>().sprite =
                                    jumpUpAndDownBeforeObjective.entities[0].GetComponent<SpriteLibrary>().GetSprite
                                    (

                                        jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentData>().information.name,
                                        jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentData>().information.name + "_" + (jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentAnimation>().lastFrameOffset + animationFrame)

                                    );

                                }
                                else
                                {

                                    jumpUpAndDownBeforeObjective.state = ObjectiveContext.State.Four;
                                    jumpUpAndDownBeforeObjective.time = restartTime;

                                }

                                break;

                            case ObjectiveContext.State.Four:

                                jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentAnimation>().animationObjective = true;

                                animationFrame = 0;

                                if (jumpUpAndDownBeforeObjective.time < jumpTime)
                                {

                                    jumpUpAndDownBeforeObjective.entities[0].GetComponent<Rigidbody2D>().velocity = -Vector2.up * jumpSpeed;

                                    jumpUpAndDownBeforeObjective.entities[0].GetComponent<SpriteRenderer>().sprite =
                                    jumpUpAndDownBeforeObjective.entities[0].GetComponent<SpriteLibrary>().GetSprite
                                    (

                                        jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentData>().information.name,
                                        jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentData>().information.name + "_" + (jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentAnimation>().lastFrameOffset + animationFrame)

                                    );

                                }
                                else
                                {

                                    jumpUpAndDownBeforeObjective.state = ObjectiveContext.State.Five;
                                    jumpUpAndDownBeforeObjective.time = restartTime;

                                    jumpUpAndDownBeforeObjective.entities[0].GetComponent<SpriteRenderer>().sprite =
                                    jumpUpAndDownBeforeObjective.entities[0].GetComponent<SpriteLibrary>().GetSprite
                                    (

                                        jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentData>().information.name,
                                        jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentData>().information.name + "_" + (jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentAnimation>().lastFrameOffset + animationFrame)

                                    );

                                }

                                break;

                            case ObjectiveContext.State.Five:

                                jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentAnimation>().animationObjective = true;

                                animationFrame = 0;

                                jumpUpAndDownBeforeObjective.entities[0].GetComponent<SpriteRenderer>().sprite =
                                    jumpUpAndDownBeforeObjective.entities[0].GetComponent<SpriteLibrary>().GetSprite
                                    (

                                        jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentData>().information.name,
                                        jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentData>().information.name + "_" + (jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentAnimation>().lastFrameOffset + animationFrame)

                                    );

                                jumpUpAndDownBeforeObjective.entities[0].GetComponent<StudentAnimation>().animationObjective = false;

                                jumpUpAndDownBeforeObjective.entities[0].GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                                Debug.Log(recordTime);

                                return true;


                        }

                    }

                    return false;

                }


            );

            GetComponent<StudentObjectivesScript>().objectives.Add(runToTeacherObjective,
                (runToTeacherObjective) =>
                {

                    runToTeacherObjective.entities[0].GetComponent<StudentAnimation>().animationObjective = false;

                    if (runToTeacherObjective.entities[0].GetComponent<NavMeshScript>() != null)
                    {

                        Transform navigationPoint = runToTeacherObjective.entities[0].GetComponent<NavMeshScript>().target;
                        Transform lastSeenTeacher = runToTeacherObjective.entities[0].GetComponent<StudentVisionScript>().lastSeenTeacher.transform;

                        navigationPoint.position = lastSeenTeacher.position;

                        if (Mathf.Abs(runToTeacherObjective.entities[0].transform.position.x - lastSeenTeacher.transform.position.x) < 50
                        && Mathf.Abs(runToTeacherObjective.entities[0].transform.position.y - lastSeenTeacher.transform.position.y) < 70)
                        {

                            runToTeacherObjective.entities[0].GetComponent<NavMeshScript>().target.position = runToTeacherObjective.entities[0].transform.position;

                            GetComponent<StudentStateRules>().state = StudentStateRules.State.Normal;


                            runToTeacherObjective.entities[0].GetComponent<StudentAnimation>().animationObjective = true;

                            return true;

                        }

                        else
                        {

                            return false;

                        }

                    }

                    else
                    {

                        return true;

                    }

                }
            );

            GetComponent<StudentObjectivesScript>().objectives.Add(jumpUpAndDownAfterObjective,
                (jumpUpAndDownAfterObjective) =>
                {

                    jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentAnimation>().animationObjective = true;

                    jumpUpAndDownAfterObjective.entities[0].GetComponent<NavMeshScript>().target.position = jumpUpAndDownAfterObjective.entities[0].transform.position;

                    if (jumpUpAndDownAfterObjective.entities[0].GetComponent<NavMeshScript>() != null)
                    {

                        Transform navigationPoint = jumpUpAndDownAfterObjective.entities[0].GetComponent<NavMeshScript>().target;

                        float jumpTime = 0.07f;
                        float jumpSpeed = 150f;

                        int witnessIndex = 0;
                        int restartTime = 0;

                        int animationFrame;


                        jumpUpAndDownAfterObjective.time += Time.deltaTime;

                        switch (jumpUpAndDownAfterObjective.state)
                        {

                            case ObjectiveContext.State.One:

                                animationFrame = 1;

                                if (jumpUpAndDownAfterObjective.time < jumpTime)
                                {

                                    jumpUpAndDownAfterObjective.entities[witnessIndex].GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpSpeed;

                                    jumpUpAndDownAfterObjective.entities[0].GetComponent<SpriteRenderer>().sprite =
                                    jumpUpAndDownAfterObjective.entities[0].GetComponent<SpriteLibrary>().GetSprite
                                    (

                                        jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentData>().information.name,
                                        jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentData>().information.name + "_" + (jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentAnimation>().lastFrameOffset + animationFrame)

                                    );

                                }
                                else
                                {

                                    jumpUpAndDownAfterObjective.state = ObjectiveContext.State.Two;
                                    jumpUpAndDownAfterObjective.time = restartTime;

                                }

                                break;

                            case ObjectiveContext.State.Two:

                                animationFrame = 2;

                                if (jumpUpAndDownAfterObjective.time < jumpTime)
                                {

                                    jumpUpAndDownAfterObjective.entities[witnessIndex].GetComponent<Rigidbody2D>().velocity = -Vector2.up * jumpSpeed;

                                    jumpUpAndDownAfterObjective.entities[0].GetComponent<SpriteRenderer>().sprite =
                                    jumpUpAndDownAfterObjective.entities[0].GetComponent<SpriteLibrary>().GetSprite
                                    (

                                        jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentData>().information.name,
                                        jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentData>().information.name + "_" + (jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentAnimation>().lastFrameOffset + animationFrame)

                                    );

                                }
                                else
                                {

                                    jumpUpAndDownAfterObjective.state = ObjectiveContext.State.Three;
                                    jumpUpAndDownAfterObjective.time = restartTime;

                                }

                                break;

                            case ObjectiveContext.State.Three:

                                animationFrame = 3;

                                if (jumpUpAndDownAfterObjective.time < jumpTime)
                                {

                                    jumpUpAndDownAfterObjective.entities[witnessIndex].GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpSpeed;

                                    jumpUpAndDownAfterObjective.entities[0].GetComponent<SpriteRenderer>().sprite =
                                    jumpUpAndDownAfterObjective.entities[0].GetComponent<SpriteLibrary>().GetSprite
                                    (

                                        jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentData>().information.name,
                                        jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentData>().information.name + "_" + (jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentAnimation>().lastFrameOffset + animationFrame)

                                    );

                                }
                                else
                                {

                                    jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentVisionScript>().lastSeenTeacher.GetComponent<NavMeshScriptTeacher>().TellOnTimmy(GameObject.Find("Timmy"));
                                    jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentVisionScript>().lastSeenTeacher.GetComponent<TeacherStateRules>().state = TeacherStateRules.State.Panic;

                                    jumpUpAndDownAfterObjective.state = ObjectiveContext.State.Four;
                                    jumpUpAndDownAfterObjective.time = restartTime;

                                }

                                break;

                            case ObjectiveContext.State.Four:

                                animationFrame = 0;

                                if (jumpUpAndDownAfterObjective.time < jumpTime)
                                {

                                    jumpUpAndDownAfterObjective.entities[witnessIndex].GetComponent<Rigidbody2D>().velocity = -Vector2.up * jumpSpeed;

                                    jumpUpAndDownAfterObjective.entities[0].GetComponent<SpriteRenderer>().sprite =
                                    jumpUpAndDownAfterObjective.entities[0].GetComponent<SpriteLibrary>().GetSprite
                                    (

                                        jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentData>().information.name,
                                        jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentData>().information.name + "_" + (jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentAnimation>().lastFrameOffset + animationFrame)

                                    );

                                }
                                else
                                {

                                    jumpUpAndDownAfterObjective.state = ObjectiveContext.State.Five;
                                    jumpUpAndDownAfterObjective.time = restartTime;

                                    jumpUpAndDownAfterObjective.entities[0].GetComponent<SpriteRenderer>().sprite =
                                    jumpUpAndDownAfterObjective.entities[0].GetComponent<SpriteLibrary>().GetSprite
                                    (

                                        jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentData>().information.name,
                                        jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentData>().information.name + "_" + (jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentAnimation>().lastFrameOffset + animationFrame)

                                    );

                                }

                                break;

                            case ObjectiveContext.State.Five:

                                animationFrame = 0;

                                jumpUpAndDownAfterObjective.entities[0].GetComponent<SpriteRenderer>().sprite =
                                jumpUpAndDownAfterObjective.entities[0].GetComponent<SpriteLibrary>().GetSprite
                                    (

                                        jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentData>().information.name,
                                        jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentData>().information.name + "_" + (jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentAnimation>().lastFrameOffset + animationFrame)

                                    );

                                jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentAnimation>().animationObjective = false;

                                jumpUpAndDownAfterObjective.entities[witnessIndex].GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                                return true;


                        }

                    }

                    return false;

                }


            );

        }

    }

    private void OnDrawGizmos()
    {

        foreach(KeyValuePair<GameObject,float> obj in studentsInView)
        {

            Gizmos.DrawLine(transform.position, obj.Key.transform.position);

        }

        foreach (KeyValuePair<GameObject, float> obj in corpsesInView)
        {

            Gizmos.DrawLine(transform.position, obj.Key.transform.position);

        }

        foreach (KeyValuePair<GameObject, float> obj in teachersInView)
        {

            Gizmos.DrawLine(transform.position, obj.Key.transform.position);

        }

        if(timmy != null)
        {

            Gizmos.DrawLine(transform.position, timmy.transform.position);

        }

    }

    List<Vector2> GeneratePositionsAroundOrigin(int count)
    {

        List<Vector2> positions = new List<Vector2>();

        for (int i = 0; i < count; i++)
        {
            float angle = i * (360f / count); // Spread the positions evenly in a circle
            float radians = Mathf.Deg2Rad * angle;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);

            Vector2 position = new Vector2(x, y);
            positions.Add(position);
        }

        return positions;
    }

}
