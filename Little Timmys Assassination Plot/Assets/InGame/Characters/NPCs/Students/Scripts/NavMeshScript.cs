using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class NavMeshScript : MonoBehaviour
{

    [SerializeField]
    Transform target;

    NavMeshAgent agent;

    StudentAnimation studentAnimation;

    Rigidbody2D rb;

    PlayerInput input;

    float stateTimer;

    float layerY;

    List<GameObject> navigationTiles;
    GameObject tileReference;


    public enum Movement
    {

        Vertical,
        Horizontal

    }

    Movement state;

    // Start is called before the first frame update
    void Awake()
    {

        navigationTiles = new List<GameObject>();

        input = new PlayerInput();

        layerY = transform.position.z;
        
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        state = Movement.Vertical;

        studentAnimation = GetComponent<StudentAnimation>();

        rb = GetComponent<Rigidbody2D>();

        foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {

            if (gameObject.name == "FloorTileReference")
            {

                tileReference = gameObject;

            }

        }

    }

    private void OnEnable()
    {

        input.Enable();

        input.InGame.RandomizeNavigation.performed += RandomizeNavigation;

        agent.Warp(new Vector3(transform.position.x, transform.position.y, layerY));

        agent.SetDestination(target.position);

        UpdateState();

    }

    void GenerateNavigationPositions()
    {

        GameObject floor = GameObject.Find("Floor");

        Tilemap tilemap = floor.GetComponent<Tilemap>();

        BoundsInt boundsInt = tilemap.cellBounds;

        Vector3 position
            = boundsInt.position;
        Vector3 size
            = boundsInt.size * 80;
        Vector3 mapPositionOffset
            = new Vector3(boundsInt.position.x, boundsInt.position.y) * 80;

        GameObject mapPositionsParent = GameObject.Find("NavigationTiles");

        tileReference.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0f);

        for (int i = 0; i < size.x; i += 80)
        {

            for (int j = 0; j < size.y; j += 80)
            {

                GameObject instance = Instantiate(tileReference, position
                    + new Vector3(i, j, 0)
                    + mapPositionOffset
                    + new Vector3(0.5f * 80, 0.5f * 80, 0), Quaternion.identity);

                instance.transform.localScale = (Vector3.one * 80);

                instance.transform.SetParent(mapPositionsParent.transform, true);

                navigationTiles.Add(instance);

            }

        }
        

    }

    void RandomizeNavigation(InputAction.CallbackContext context)
    {

        int minIndex = 0;
        int maxIndex = navigationTiles.Count-1;

        int randomIndex = Random.Range(minIndex, maxIndex);

        target.gameObject.transform.position = new Vector3(navigationTiles[randomIndex].transform.position.x, navigationTiles[randomIndex].transform.position.y, transform.position.z);


        if (agent.CalculatePath(target.position, agent.path))
        {

            Debug.Log("Path Found!");

        }
        else
        {

            RandomizeNavigation(context);

        }

    }

    private void Start()
    {

        agent.SetDestination(target.position);

        UpdateState();

        GenerateNavigationPositions();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(GetComponent<StudentAnimation>() != null)
        {

            if(GetComponent<StudentAnimation>().lifeState == StudentAnimation.LifeState.Dead)
            {

                rb.velocity = Vector3.zero;

                return;

            }

        }

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
