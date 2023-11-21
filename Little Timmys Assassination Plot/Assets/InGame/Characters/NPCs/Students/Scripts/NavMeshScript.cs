using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshScript : MonoBehaviour
{

    [SerializeField]
    Transform target;

    NavMeshAgent agent;

    StudentAnimation studentAnimation;

    Rigidbody2D rb;

    float stateTimer;

    float layerY;

    public enum Movement
    {

        Vertical,
        Horizontal

    }

    Movement state;

    // Start is called before the first frame update
    void Awake()
    {

        layerY = transform.position.z;
        
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        state = Movement.Vertical;

        studentAnimation = GetComponent<StudentAnimation>();

        rb = GetComponent<Rigidbody2D>();

    }

    private void OnEnable()
    {

        agent.Warp(new Vector3(transform.position.x, transform.position.y, layerY));

        agent.SetDestination(target.position);

        UpdateState();
    }

    private void Start()
    {

        agent.SetDestination(target.position);

        UpdateState();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Timers();

        if (stateTimer > 0.4f)
        {

            agent.SetDestination(target.position);

            stateTimer = 0;

            UpdateState();


        }

        if(Mathf.Abs(transform.position.x - target.transform.position.x) < 40 && Mathf.Abs(transform.position.y - target.transform.position.y) < 15)
        {

            rb.velocity = new Vector3(0, 0, 0);

            studentAnimation.isMoving = false;

            return;

        }
        else
        {

            studentAnimation.isMoving = true;

        }

        if (state == Movement.Horizontal)
        {

            if(agent.desiredVelocity.x > 0)
            {

                rb.velocity = new Vector3(40, 0, 0);

                studentAnimation.state = StudentAnimation.AnimationState.Right;

            }
            else
            {

                rb.velocity = new Vector3(-40, 0, 0);

                studentAnimation.state = StudentAnimation.AnimationState.Left;
            }


        }
        else if (state == Movement.Vertical)
        {

            if(agent.desiredVelocity.y > 0)
            {

                rb.velocity = new Vector3(0, 40, 0);

                studentAnimation.state = StudentAnimation.AnimationState.Up;

            }
            else
            {

                rb.velocity = new Vector3(0, -40, 0);

                studentAnimation.state = StudentAnimation.AnimationState.Down;

            }

        }

    }

    void Timers()
    {

        stateTimer += Time.deltaTime;



    }

    void UpdateState()
    {


        if (Mathf.Abs(agent.desiredVelocity.normalized.x) > Mathf.Abs(agent.desiredVelocity.normalized.y))
        {

            state = Movement.Horizontal;


        }
        else
        {

            state = Movement.Vertical;

        }

    }

    [BurstCompile]
    private void Update()
    {

        agent.velocity = Vector3.zero;

    }
}
