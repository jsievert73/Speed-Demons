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
    public GameObject visualModel;
    private Vector3 forceDirection = new Vector3(0,0,0);
    public Unit referenceUnit;
    private bool HasUpdated = false;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 sample1 = new Vector3(0,0,0); 
        Vector3 sample2 = new Vector3(50,0,20);
        Vector3 sample3 = new Vector3(0,0,30);
        currentPosition = transform.position;
        //waypoints.Add(sample1);
        //waypoints.Add(sample2);
        //waypoints.Add(sample3);
        thisEnemy = GetComponent<Rigidbody>();
        print(referenceUnit.tileX);
        print(referenceUnit.currentPath);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!HasUpdated)
        {
            foreach (Node waypoint in referenceUnit.currentPath)
            {
                Vector3 sampler = new Vector3(0,0,-5);
                sampler.x = waypoint.x;
                sampler.y = waypoint.y;
                waypoints.Add(sampler);
            }
            HasUpdated = true;
        }
        if (targetWaypoint.x > 5000)
        {
            print("CHECK: "+ waypointCount);
            targetWaypoint = waypoints[waypointCount];
            waypointCount += 1;
        }
        else
        {
            print("POSITION" + transform.position.x + "targetPosition"+ targetWaypoint.x);
            if(Mathf.Abs(transform.position.x - targetWaypoint.x) < 0.7)
            {
                finishX = true;
                print("FINISH X");
            }
            else if(transform.position.x > targetWaypoint.x)
            {
                if(thisEnemy.velocity.x > 0)
                {
                    forceDirection.x = -5;
                }
                else
                {
                    forceDirection.x = -2;
                    finishX = false;
                }
            }
            else
            {
                if(thisEnemy.velocity.x < 0)
                {
                    forceDirection.x = 5;
                }
                forceDirection.x = 2;
                finishX = false;
            }
            if(Mathf.Abs(transform.position.y - targetWaypoint.y) < 0.7)
            {
                finishY = true;
                print("FINISH Y");
            }
            else if(transform.position.y > targetWaypoint.y)
            {
                if(thisEnemy.velocity.y > 0)
                {
                    forceDirection.y = -5;
                }
                forceDirection.y = -2;
                finishY = false;
            }
            else
            {
                if(thisEnemy.velocity.y < 0)
                {
                    forceDirection.y = 5;
                }
                forceDirection.y = 2;
                finishY = false;
            }
            if(finishX && finishY)
            {
                targetWaypoint = Vector3.positiveInfinity;
                print("NEW WAYPOINT");
                finishX = false;
                finishY = false;
                thisEnemy.velocity = new Vector3(0,0,0);
            }
            thisEnemy.AddForce(forceDirection, ForceMode.Acceleration);
        }
        float calculatedRotation = 0;
        if(thisEnemy.velocity.x > 0 )
        {
            if(thisEnemy.velocity.y > 0)
            {
                calculatedRotation = 90-(Mathf.Rad2Deg*Mathf.Atan(thisEnemy.velocity.y/thisEnemy.velocity.x));
            }
            else
            {
                calculatedRotation = 180+(Mathf.Rad2Deg*Mathf.Atan(thisEnemy.velocity.y/thisEnemy.velocity.x));
            }
        }
        else
        {
            if(thisEnemy.velocity.y > 0)
            {
                calculatedRotation = 270-(Mathf.Rad2Deg*Mathf.Atan(thisEnemy.velocity.y/thisEnemy.velocity.x));
            }
            else
            {
                calculatedRotation = 180+(Mathf.Rad2Deg*Mathf.Atan(thisEnemy.velocity.y/thisEnemy.velocity.x));
            }
        }
        visualModel.transform.eulerAngles = new Vector3(0,0,calculatedRotation);
    }
}
