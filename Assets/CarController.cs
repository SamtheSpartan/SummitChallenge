using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CarController : MonoBehaviour
{
    public GhostCarController ghostCar;
    public float maxSpeed;
    public float distance;
    float angle;
    float angleSign;
    float angleAllowance = 0.1f;
    public float minDistance = 2f;
    public int waypoint = 0;

    // Use this for initialization
    void Start ()
    {
    }

    // Update is called once per frame
    void Update ()
    {
        //distance = Vector3.Distance(ghostCar.transform.position, transform.position);
        //angle = Vector3.Angle(transform.forward, ghostCar.transform.position - transform.position);
        //float local = transform.InverseTransformPoint(ghostCar.transform.position).z;
        ////looks like z axis is going to be the check
        //angleSign = angle * local;
        //AdjustMovement();

        distance = Vector3.Distance(ghostCar.waypoints[waypoint], transform.position);
        angle = Vector3.Angle(transform.forward, ghostCar.waypoints[waypoint] - transform.position);
        float local = transform.InverseTransformPoint(ghostCar.waypoints[waypoint]).z;
        //looks like z axis is going to be the check
        angleSign = angle * local;
        
        AdjustMovement();
        if (Vector3.Distance(ghostCar.waypoints[waypoint], transform.position) <= 0.5f)
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
        if(distance < minDistance)
        {
            GoForward(distance);
        }
        //if(distance >= minDistance)
        //{
        //    GoBackward(distance);
        //}
    }
    void TurnLeft(float dist)
    {
        transform.Rotate(0, dist * Time.deltaTime, 0);
    }
    void TurnRight(float dist)
    {
        transform.Rotate(0, dist * Time.deltaTime, 0);
    }
    void GoForward(float dist)
    {
        transform.Translate(transform.forward * Time.deltaTime);
    }
    void GoBackward(float dist)
    {
        transform.Translate(-transform.forward * Time.deltaTime);
    }
}
