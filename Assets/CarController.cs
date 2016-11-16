using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CarController : MonoBehaviour
{
    public float maxSpeed;
    public float distance;
    public float angle;
    public float angleSign;
    public float angleAllowance = 0.1f;
    public float minDistance = 2f;
    public GameObject[] waypoints;
    public int waypoint = 0;
    public float rotateSpeed;

    void Start ()
    {
    }

    void Update ()
    {
        distance = Vector3.Distance(waypoints[waypoint].transform.position, transform.position);
        angle = Vector3.Angle(transform.forward, waypoints[waypoint].transform.position - transform.position);
        float local = transform.InverseTransformPoint(waypoints[waypoint].transform.position).z;
        //looks like z axis is going to be the check
        angleSign = angle * local;
        
        AdjustMovement();
        if (distance <= minDistance)
        {
            waypoint++;
        }
    }
    void AdjustMovement()
    {
        if(angleSign >= angleAllowance)
        {
            TurnRight(angleSign);
        }
        if (angleSign < angleAllowance * -1)
        {
            TurnLeft(angleSign);
        }
        if(distance > minDistance)
        {
            GoForward(distance);
        }
        
    }
    void TurnLeft(float dist)
    {
        transform.Rotate(0, dist * Time.deltaTime * rotateSpeed, 0);
    }
    void TurnRight(float dist)
    {
        transform.Rotate(0, dist * Time.deltaTime * rotateSpeed, 0);
    }
    void GoForward(float dist)
    {
        transform.Translate(transform.forward * Time.deltaTime * maxSpeed);
    }
    void GoBackward(float dist)
    {
        transform.Translate(-transform.forward * Time.deltaTime * maxSpeed);
    }
}
