using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GhostCarController : MonoBehaviour
{
    public GameObject dest;
    private UnityEngine.AI.NavMeshAgent nav;
    public GameObject car;
    public float maxDistance;
    public float maxSpeed;
    public List<Vector3> waypoints = new List<Vector3>();
    float maxWaypointSpawnTime = 0.5f;
    float wayPointSpawnTimer = 0.5f;

    void Start ()
    {
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        CreateWaypoint();
    }

    void Update ()
    {
        //RaycastHit hit;
        //if(Physics.Raycast(transform.position, car.transform.position - this.transform.position, out hit))
        //{
        //    if(hit.collider.gameObject.tag == "Car")
        //    {
        //        nav.destination = dest.transform.position;
        //        float distance = Vector3.Distance(car.transform.position, transform.position);
        //        float percentage = Mathf.InverseLerp(maxDistance, 0, distance);
        //        nav.speed = maxSpeed * percentage;
        //    }
        //    else
        //    {
        //        nav.destination = car.transform.position;
        //        //float distance = Vector3.Distance(car.transform.position, transform.position);
        //        //float percentage = Mathf.InverseLerp(maxDistance, 0, distance);
        //        nav.speed = maxSpeed;// * percentage;
        //    }
        //    Debug.Log(hit.collider.gameObject.name);
        //}
        wayPointSpawnTimer -= Time.deltaTime;
        nav.destination = dest.transform.position;
        float distance = Vector3.Distance(car.transform.position, transform.position);
        float percentage = Mathf.InverseLerp(maxDistance, 0, distance);
        nav.speed = maxSpeed * percentage;
        if (wayPointSpawnTimer <= 0 && Vector3.Distance(transform.position, waypoints[waypoints.Count - 1]) > 0.1f)
        {
            CreateWaypoint();
        }
    }
    void CreateWaypoint()
    {
        waypoints.Add(transform.position);
        wayPointSpawnTimer = maxWaypointSpawnTime;
    }
}
