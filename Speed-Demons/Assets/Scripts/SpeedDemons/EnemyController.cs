using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    List<Vector3> waypoints = new List<Vector3>(); 
    int waypointCount = 0;
    private Vector3 targetWaypoint = Vector3.positiveInfinity;
    private Vector3 currentPosition = new Vector3(0,0,0);
    private Rigidbody thisEnemy = null;
    bool finishX = false;
    bool finishY = false;
    private Vector3 forceDirection = new Vector3(0,0,0);
    // Start is called before the first frame update
    void Start()
    {
        Vector3 sample1 = new Vector3(0,0,0); 
        Vector3 sample2 = new Vector3(50,0,20);
        currentPosition = transform.position;
        waypoints.Add(sample1);
        waypoints.Add(sample2);
        thisEnemy = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetWaypoint.x > 5000)
        {
            print("CHECK: "+ waypointCount);
            targetWaypoint = waypoints[waypointCount];
            waypointCount += 1;
        }
        else
        {
            print("POSITION" + transform.position.x + "targetPosition"+ targetWaypoint.x);
            if(Mathf.Abs(transform.position.x - targetWaypoint.x) < 5)
            {
                finishX = true;
                print("FINISH X");
            }
            else if(transform.position.x > targetWaypoint.x)
            {
                forceDirection.x = -2;
                finishX = false;
            }
            else
            {
                forceDirection.x = 2;
                finishX = false;
            }
            if(Mathf.Abs(transform.position.z - targetWaypoint.z) < 5)
            {
                finishY = true;
                print("FINISH Y");
            }
            else if(transform.position.z > targetWaypoint.z)
            {
                forceDirection.z = -2;
                finishY = false;
            }
            else
            {
                forceDirection.z = 2;
                finishY = false;
            }
            if(finishX && finishY)
            {
                targetWaypoint = Vector3.positiveInfinity;
                print("NEW WAYPOINT");
                finishX = false;
                finishY = false;
            }
            thisEnemy.AddForce(forceDirection, ForceMode.Acceleration);
        }
    }
}
