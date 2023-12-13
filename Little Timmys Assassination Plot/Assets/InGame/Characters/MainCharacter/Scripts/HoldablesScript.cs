using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;
using static UnityEngine.GraphicsBuffer;

public class HoldablesScript : MonoBehaviour
{

    PlayerInput input;

    bool pickingUp = false;

    public GameObject dragingHuman;
    public GameObject heldItem;
    public GameObject[] heldSubItems;

    List<GameObject> collidingStudents;

    // Start is called before the first frame update
    void Awake()
    {
        
        collidingStudents = new List<GameObject>();

        input = new PlayerInput();

        heldItem = null;
        heldSubItems = new GameObject[3];

    }

    private void OnEnable()
    {

        input.Enable();
        input.InGame.PickUp.performed += PickUpInput;
        

    }

    int studentsInColliderIterator = 0;

    void KillStudent()
    {

        if(studentsInColliderIterator >= collidingStudents.Count)
        {

            return;

        }

        if (collidingStudents[studentsInColliderIterator].GetComponentInChildren<StudentData>() != null 
            && collidingStudents[studentsInColliderIterator].GetComponentInChildren<StudentData>().information.lifeState != StudentAnimation.LifeState.Dead)
        {

            collidingStudents[studentsInColliderIterator].GetComponentInChildren<StudentData>().information.lifeState = StudentAnimation.LifeState.Dead;
            collidingStudents[studentsInColliderIterator].GetComponentInChildren<StudentData>().gameObject.layer = LayerMask.NameToLayer("Corpses");
            collidingStudents[studentsInColliderIterator].GetComponentInChildren<StudentStateRules>().UpdateCollider();
            collidingStudents[studentsInColliderIterator].GetComponentInChildren<StudentVisionScript>().hasVision = false;

            GameObject.Find("oMeter").GetComponent<oMeter>().angervalue = 9.9f;
            GameObject.Find("oMeter").GetComponent<oMeter>().corpses.Add(collidingStudents[studentsInColliderIterator].GetComponentInChildren<StudentData>().gameObject);

            GameObject killedStudent = collidingStudents[studentsInColliderIterator];

            if (killedStudent.GetComponentInChildren<StudentVisionScript>().studentsInView.Keys.Count > 0)
            {

                foreach (GameObject witness in killedStudent.GetComponentInChildren<StudentVisionScript>().studentsInView.Keys)
                {

                    if (witness.GetComponent<StudentAnimation>() != null)
                    {

                        if (witness.GetComponent<StudentAnimation>().lifeState == StudentAnimation.LifeState.Dead)
                        {

                            continue;

                        }

                    }

                    witness.GetComponent<StudentStateRules>().state = StudentStateRules.State.Panic;


                    GameObject[] objectiveEntities = new GameObject[1] { witness };
                    GameObject allertSignia = Instantiate(witness.GetComponent<StudentData>().alertSignia);
                    allertSignia.transform.position = witness.transform.position + new Vector3(0, 40, 0);

                    ObjectiveContext jumpUpAndDownBeforeObjective = new ObjectiveContext(objectiveEntities, ObjectiveContext.State.One, ObjectiveContext.GenerateID());
                    ObjectiveContext runToTeacherObjective = new ObjectiveContext(objectiveEntities, ObjectiveContext.GenerateID());
                    ObjectiveContext jumpUpAndDownAfterObjective = new ObjectiveContext(objectiveEntities, ObjectiveContext.State.One, ObjectiveContext.GenerateID());

                    float recordTime = 0;

                    witness.GetComponent<StudentObjectivesScript>().objectives.Add(jumpUpAndDownBeforeObjective,
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

                                        return true;


                                }

                            }

                            return false;

                        }


                    );

                    witness.GetComponent<StudentObjectivesScript>().objectives.Add(runToTeacherObjective,
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

                                    witness.GetComponent<StudentStateRules>().state = StudentStateRules.State.Normal;


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

                    witness.GetComponent<StudentObjectivesScript>().objectives.Add(jumpUpAndDownAfterObjective,
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

                                            jumpUpAndDownAfterObjective.entities[0].GetComponent<StudentVisionScript>().lastSeenTeacher.GetComponent<NavMeshScriptTeacher>().TellOnTimmy(gameObject);
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

        }
        else
        {

            studentsInColliderIterator++;
            KillStudent();

        }

    }

    void PickUpInput(InputAction.CallbackContext context)
    {

        if(context.ReadValue<float>() == 1)
        {

            if (heldItem != null)
            {

                if (collidingStudents.Count > 0)
                {

                    studentsInColliderIterator = 0;
                    KillStudent();

                    return;

                }

            }

        }

        if(context.ReadValue<float>() == 1)
        {


            pickingUp = true;

        }
        else
        {

            pickingUp = false;

        }


    }

    // Update is called once per frame
    void Update()
    {

        DropItem();
        HoldItem();
        HoldHuman();

    }

    public Vector3 lastHeldHumanPosition = new Vector3();

    void HoldHuman()
    {

        if(dragingHuman != null)
        {

            Vector3 getHoldingPosition = ChangeHeldBodyBodyPosition(dragingHuman.transform.position.z);

            if(getHoldingPosition.x != 0 && getHoldingPosition.y != 0)
            {

                lastHeldHumanPosition = getHoldingPosition;

            }

            dragingHuman.gameObject.transform.position = lastHeldHumanPosition;

        }

    }

    public Vector3 ChangeHeldBodyBodyPosition(float zDistance)
    {

        float xDistance = 20;
        float yDistance = 20;

        float posX = transform.position.x;
        float posY = transform.position.y;

        if (GetComponent<playerController>().walkUp && GetComponent<playerController>().walkLeft)
        {

            return new Vector3(-xDistance + posX, yDistance + posY, zDistance);
        }

        if (GetComponent<playerController>().walkDown && GetComponent<playerController>().walkLeft)
        {

            return new Vector3(-xDistance + posX, -yDistance + posY, zDistance);
        }

        if (GetComponent<playerController>().walkUp && GetComponent<playerController>().walkRight)
        {

            return new Vector3(xDistance + posX, yDistance + posY, zDistance);
        }

        if (GetComponent<playerController>().walkDown && GetComponent<playerController>().walkRight)
        {

            return new Vector3(xDistance + posX, -yDistance + posY, zDistance);
        }

        if (GetComponent<playerController>().walkLeft)
        {

            return new Vector3(-xDistance + posX, 0 + posY, zDistance);
        }

        if (GetComponent<playerController>().walkUp)
        {

            return new Vector3(0 + posX, yDistance + posY, zDistance);
        }

        if (GetComponent<playerController>().walkDown)
        {

            return new Vector3(0 + posX, -yDistance + posY, zDistance);
        }

        if (GetComponent<playerController>().walkRight)
        {

            return new Vector3(xDistance + posX, 0 + posY, zDistance);
        }

        return new Vector3(0, 0, 0);

    }

    void HoldItem()
    {

        if(heldItem != null)
        {

            heldItem.transform.position = new Vector3(transform.position.x, transform.position.y + 25, heldItem.transform.position.z);
            heldItem.GetComponent<SpriteRenderer>().enabled = false;

        }

        foreach(GameObject instance in heldSubItems)
        {

            if(instance != null)
            {

                if (instance.name.Contains("Coin"))
                {
                    continue;
                }

                instance.GetComponent<SpriteRenderer>().enabled = false;
                instance.GetComponent<Collider2D>().enabled = false;

            }

        }

    }

    void DropItem()
    {

        if (pickingUp && heldItem != null)
        {

            heldItem.GetComponent<SpriteRenderer>().enabled = true;
            heldItem = null;

        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (pickingUp && collision.gameObject.tag.Equals("Object"))
        {

            heldItem = collision.gameObject;

            pickingUp = false;

        }

        if(pickingUp && collision.gameObject.tag.Equals("SubObject"))
        {

            int i = 0;

            foreach(GameObject instance in heldSubItems)
            {

                if(instance == null)
                {

                    heldSubItems[i] = collision.gameObject;

                    break;

                }

                i++;

            }

            pickingUp = false;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.name.Contains("Body"))
        {

            collidingStudents.Add(collision.gameObject.transform.parent.gameObject);

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.name.Contains("Body"))
        {

            collidingStudents.Remove(collision.gameObject.transform.parent.gameObject);

        }

    }

}
