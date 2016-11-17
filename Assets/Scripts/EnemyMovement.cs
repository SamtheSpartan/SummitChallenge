using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    //PlayerHealth playerHealth;
    //EnemyHealth enemyHealth;

    float speed = 0.25f;
    float turnSpeed = 1.050f;
    private Vector3 direction;


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Car").transform;
        speed = Random.Range(0.25f, 0.75f);
        //playerHealth = player.GetComponent <PlayerHealth> ();
        //enemyHealth = GetComponent <EnemyHealth> ();
        //nav = GetComponent <NavMeshAgent> ();
    }


    void Update ()
    {
        float step = speed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        direction = player.position - transform.position;
        transform.position += direction * speed * Time.deltaTime;

        direction.Normalize();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
    }
}
