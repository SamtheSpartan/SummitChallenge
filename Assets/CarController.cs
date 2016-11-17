using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CarController : MonoBehaviour
{
    public float maxSpeed;
    public float rotateSpeed;
    public float distance;
    public float angle;
    public float angleSign;
    public float angleAllowance = 0.1f;
    public float stopToOrientAngle = 90f;
    public float minDistance = 2f;
    public GameObject[] waypoints;
    public int waypoint = 0;
    public enum NavState { PATHFINDING, ORIENTING, STOPPED}
    public NavState navState = NavState.PATHFINDING;
    void Start ()
    {
    }

    void Update ()
    {
        if (distance <= minDistance && waypoint < waypoints.Length - 1)
        {
            waypoint++;
            if(waypoint == waypoints.Length)
            {
                navState = NavState.STOPPED;
            }
        }
        //Determine nav data
        distance = Vector3.Distance(waypoints[waypoint].transform.position, transform.position);
        angle = Vector3.Angle(this.transform.forward, waypoints[waypoint].transform.position - transform.position);
        float local = Mathf.Sign(transform.InverseTransformPoint(waypoints[waypoint].transform.position).x);
        Debug.Log(local);
        //looks like z axis is going to be the check
        angleSign = angle * local;
        
        
        switch (navState)
        {
            case NavState.PATHFINDING:
                if (distance > minDistance)
                {
                    GoForward();
                }
               
                Turn(angleSign);
                if(angle > stopToOrientAngle)
                {
                    navState = NavState.ORIENTING;
                }
                break;
            case NavState.ORIENTING:
                
                Turn(-angleSign);
                if(angle <= angleAllowance)
                {
                    navState = NavState.PATHFINDING;
                }
                break;
            case NavState.STOPPED:

                break;
        }
    }
  
    void Turn(float angle)
    {
        transform.Rotate(0, angle * Time.deltaTime * rotateSpeed, 0);
    }
    void TurnLeft(float dist)
    {
        transform.Rotate(0, dist * Time.deltaTime * rotateSpeed, 0);

    }
    void TurnRight(float dist)
    {
        transform.Rotate(0, dist * Time.deltaTime * rotateSpeed, 0);

    }
    void GoForward()
    {
        transform.position = transform.position + transform.forward * maxSpeed * Time.deltaTime * (distance + 0.5f);
    }
    void GoBackward()
    {
        transform.Translate(-transform.forward * Time.deltaTime * maxSpeed);
    }
}
