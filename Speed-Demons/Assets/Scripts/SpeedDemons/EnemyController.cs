using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    List<Vector3> waypoints = new List<Vector3>(); 
    int waypointCount = 0;
    public Vector3 targetWaypoint = Vector3.positiveInfinity;
    private Vector3 currentPosition = new Vector3(0,0,0);
    private Rigidbody thisEnemy = null;
    bool finishX = false;
    bool finishY = false;
    public GameObject visualModel;
    private Vector3 forceDirection = new Vector3(0,0,0);
    public Unit referenceUnit;
    public float currentSpeed = 2f;
    private bool HasUpdated = false;
    public TileMap map;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 sample1 = new Vector3(0,0,0); 
        Vector3 sample2 = new Vector3(50,0,20);
        Vector3 sample3 = new Vector3(0,0,30);
        currentPosition = transform.position;
        DemonSpawner.UpdateDemon(this);
        //waypoints.Add(sample1);
        //waypoints.Add(sample2);
        //waypoints.Add(sample3);
        thisEnemy = GetComponent<Rigidbody>();
        referenceUnit.tileX = 0;
        referenceUnit.tileY = 0;
        referenceUnit.UpdateHousing();
    }

    // Update is called once per frame
    void Update()
    {
        if(!HasUpdated)
        {
            map.GeneratePathTo(8,8,referenceUnit);
            foreach (Node waypoint in referenceUnit.currentPath)
            {
                Vector3 sampler = new Vector3(0,0,-5);
                sampler.x = waypoint.x+0.5f;
                sampler.y = waypoint.y-0.5f;
                waypoints.Add(sampler);
            }
            HasUpdated = true;
        }
        if (targetWaypoint.x > 5000)
        {
            //print("CHECK: "+ waypointCount);
            targetWaypoint = waypoints[waypointCount];
            waypointCount += 1;
        }
        else
        {
            //print("POSITION" + transform.position.x + "targetPosition"+ targetWaypoint.x);
            if(Mathf.Abs(transform.position.x - targetWaypoint.x) < 0.3 || finishX)
            {
                finishX = true;
                //print("FINISH X");
                forceDirection.x = 0;
            }
            else if(transform.position.x > targetWaypoint.x)
            {
                if(thisEnemy.velocity.x > 0)
                {
                    forceDirection.x = -currentSpeed-3;
                }
                else
                {
                    forceDirection.x = -currentSpeed;
                }
            }
            else
            {
                if(thisEnemy.velocity.x < 0)
                {
                    forceDirection.x = currentSpeed +3;
                }
                forceDirection.x = currentSpeed;
                finishX = false;
            }
            if(Mathf.Abs(transform.position.y - targetWaypoint.y) < 0.3 || finishY)
            {
                finishY = true;
                //print("FINISH Y");
                forceDirection.y = 0;
            }
            else if(transform.position.y > targetWaypoint.y)
            {
                if(thisEnemy.velocity.y > 0)
                {
                    forceDirection.y = -currentSpeed-3;
                }
                forceDirection.y = -currentSpeed;
            }
            else
            {
                if(thisEnemy.velocity.y < 0)
                {
                    forceDirection.y = currentSpeed+3;
                }
                forceDirection.y = currentSpeed;
            }
            if(finishX && finishY)
            {
                targetWaypoint = Vector3.positiveInfinity;
                //print("NEW WAYPOINT");
                finishX = false;
                finishY = false;
                //thisEnemy.velocity = new Vector3(0,0,0);
            }
            Vector3 normalizedDirection = new Vector3(forceDirection.x,forceDirection.y,0);
            //normalizedDirection = Vector3.Normalize(thisEnemy.position - targetWaypoint);
            //normalizedDirection = normalizedDirection * 5;
            //normalizedDirection.z = 0;
            //print(normalizedDirection.x + "AND" + normalizedDirection.y);
            thisEnemy.velocity = normalizedDirection;
            //thisEnemy.AddForce(forceDirection, ForceMode.Acceleration);
        }
        /*float calculatedRotation = 0;
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
        visualModel.transform.eulerAngles = new Vector3(0,0,calculatedRotation);*/
    }

    public void UpdatePath(List<Node> path)
    {
        waypoints = new List<Vector3>();
            foreach (Node waypoint in path)
            {
                Vector3 sampler = new Vector3(0,0,-5);
                sampler.x = waypoint.x+0.5f;
                sampler.y = waypoint.y-0.5f;
                waypoints.Add(sampler);
            }
    }
}
