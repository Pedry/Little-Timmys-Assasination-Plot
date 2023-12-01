using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;

public class StudentVisionScript : MonoBehaviour
{

    public Dictionary<GameObject, float> studentsInView = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> teachersInView = new Dictionary<GameObject, float>();
    public GameObject lastSeenTeacher;

    int visionResolution;

    // Start is called before the first frame update
    void Awake()
    {

        visionResolution = 40;

    }

    [BurstCompile(CompileSynchronously = false)]
    // Update is called once per frame
    void FixedUpdate()
    {

        studentsInView.Clear();
        teachersInView.Clear();

        foreach (Vector2 view in GeneratePositionsAroundOrigin(visionResolution))
        {


            int layer_mask = LayerMask.GetMask("lol", "Vision");



            RaycastHit2D hit = Physics2D.Raycast(transform.position, view, 100000, layer_mask);

            if(hit.collider == null)
            {
                continue;
            }

            if (hit.collider.gameObject.tag.Equals("Student"))
            {


                if (hit.collider.gameObject.GetComponent<StudentData>())
                {

                    if(!studentsInView.ContainsKey(hit.collider.gameObject))
                    {

                        studentsInView.Add(hit.collider.gameObject, 0);

                    }

                }

            }

            

            if(hit.collider.gameObject.transform.parent.transform.parent != null)
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

    [BurstCompile(CompileSynchronously = false)]
    private void OnDrawGizmos()
    {

        foreach(KeyValuePair<GameObject,float> obj in studentsInView)
        {

            Gizmos.DrawLine(transform.position, obj.Key.transform.position);

        }

        foreach (KeyValuePair<GameObject, float> obj in teachersInView)
        {

            Gizmos.DrawLine(transform.position, obj.Key.transform.position);

        }

    }

    [BurstCompile(CompileSynchronously = false)]
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
