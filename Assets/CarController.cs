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
    public enum NavState { SETUP, PATHFINDING, ORIENTING, STOPPED, WAYPOINTPAUSE, ATTACKED, REVERSE}
    public NavState navState = NavState.SETUP;
    private bool gameOver;
    private float stopCarTimer = 2f;
    private float waypointPauseTimer = 1f;
    public bool isDebugMode;
    public GameObject[] wallCheckers;
    public float reverseTimer = 0.75f;
    public Rigidbody mario;
    public Transform endPoint;
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
                mario.gameObject.transform.parent = null;
                LaunchMario(mario, endPoint, 3f);
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
                for (int i = 0; i < wallCheckers.Length; i++)
                {
                    RaycastHit hit;
                    if(Physics.Raycast(wallCheckers[i].transform.position, wallCheckers[i].transform.forward, out hit, 0.1f))
                    {
                        if(hit.collider != null)
                        {
                            navState = NavState.REVERSE;
                        }
                    }
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
            case NavState.ATTACKED:
                stopCarTimer -= Time.deltaTime;
                if(stopCarTimer <= 0)
                {
                    stopCarTimer = 2f;
                    navState = NavState.PATHFINDING;
                }
                break;
            case NavState.REVERSE:
                reverseTimer -= Time.deltaTime;
                if(reverseTimer <= 0)
                {
                    reverseTimer = 0.75f;
                    navState = NavState.ORIENTING;
                }
                GoBackward();
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
        navState = NavState.ATTACKED;
        stopCarTimer = 2f;
    }
    IEnumerator WebCall(string url)
    {
        WWW www = new WWW("http://172.20.120.236/" + url);
        yield return www;
        //Renderer renderer = GetComponent<Renderer>();
        //renderer.material.mainTexture = www.texture;
    }
    void LaunchMario(Rigidbody mario, Transform target, float maxHeight)
    {
        mario.transform.parent = null;
        Vector3 deltaPos = target.position - mario.transform.position;
        float h0 = deltaPos.y + maxHeight;  //max height in parabolic arc
        float h1 = maxHeight;       //height dropped from peak
        if (h0 < 0)
        {
            h0 = 0;
            h1 = mario.transform.position.y - target.position.y;
        }
        float g = -Physics.gravity.y;
        Vector3 deltaPosXZ = new Vector3(deltaPos.x, 0, deltaPos.z);
        float distanceXZ = Mathf.Abs(deltaPosXZ.magnitude); //distance along ground
        float v0y = Mathf.Sqrt(2 * g * h0); //y velocity needed to reach peak of arc
        float sec = Mathf.Sqrt(2 * h0 / g) + Mathf.Sqrt(2 * h1 / g); //seconds spent in air
        float v0xz = distanceXZ / sec; //velocity in the x and z direction

        mario.isKinematic = false;
        mario.velocity = deltaPosXZ.normalized * v0xz + Vector3.up * v0y;
    }
}
