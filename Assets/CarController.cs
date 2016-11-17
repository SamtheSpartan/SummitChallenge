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
    public enum NavState { SETUP, PATHFINDING, ORIENTING, STOPPED, WAYPOINTPAUSE}
    public NavState navState = NavState.SETUP;
    private bool gameOver;
    private float stopCarTimer = 3f;
    private float waypointPauseTimer = 1f;
    public bool isDebugMode;
    void Start ()
    {
        StartCoroutine(WebCall("stop"));

    }

    void Update ()
    {
        //check waypoint distance
        if (distance <= minDistance && waypoint < waypoints.Length - 1)
        {
            waypoint++;
            if(waypoint == waypoints.Length)
            {
                navState = NavState.STOPPED;
            }
            else
            {
                StartCoroutine(WebCall("stop"));
                navState = NavState.WAYPOINTPAUSE;
            }
        }
        //Determine nav data
        if(navState != NavState.STOPPED)
        {
            distance = Vector3.Distance(waypoints[waypoint].transform.position, transform.position);
            angle = Vector3.Angle(this.transform.forward, waypoints[waypoint].transform.position - transform.position);
            float local = Mathf.Sign(transform.InverseTransformPoint(waypoints[waypoint].transform.position).x);
            angleSign = angle * local;
        }
        
        
        
        switch (navState)
        {
            case NavState.SETUP:

                break;
            case NavState.PATHFINDING:
                if (distance > minDistance)
                {
                    GoForward();
                }
               
                //Turn(angleSign);
                if(angle > stopToOrientAngle)
                {
                    navState = NavState.ORIENTING;
                }
                break;
            case NavState.ORIENTING:
                
                Turn(angleSign);
                if(angle <= angleAllowance)
                {
                    navState = NavState.PATHFINDING;
                }
                break;
            case NavState.STOPPED:
                StartCoroutine(WebCall("stop"));
                break;
            case NavState.WAYPOINTPAUSE:
                waypointPauseTimer -= Time.deltaTime;
                if(waypointPauseTimer <= 0)
                {
                    waypointPauseTimer = 1f;
                    navState = NavState.PATHFINDING;
                }
                break;
        }
    }
  
    void Turn(float angle)
    {
        if(isDebugMode)
        {
            transform.Rotate(0, angle * Time.deltaTime * rotateSpeed, 0);
        }
        
        if(angle >= 0)
        {
            StartCoroutine(WebCall("right"));
        }
        else
        {
            StartCoroutine(WebCall("left"));
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
    void GoForward()
    {
        if (isDebugMode)
        {
            transform.position = transform.position + transform.forward * maxSpeed * Time.deltaTime * (distance + 0.5f);
        }
        StartCoroutine(WebCall("forward"));
    }
    void GoBackward()
    {
        if (isDebugMode)
        {
            transform.Translate(-transform.forward * Time.deltaTime * maxSpeed);
        }
        StartCoroutine(WebCall("reverse"));

    }
    public void StopCar()
    {
        navState = NavState.STOPPED;
        stopCarTimer = 3f;
    }
    IEnumerator WebCall(string url)
    {
        WWW www = new WWW("http://172.20.120.236/" + url);
        yield return www;
        //Renderer renderer = GetComponent<Renderer>();
        //renderer.material.mainTexture = www.texture;
    }
}
