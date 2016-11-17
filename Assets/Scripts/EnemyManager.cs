using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //public PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;
    public float radius = 0;

    private Vector3 spawnPosition;


    void Start ()
    {
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
        radius = Random.Range(2, 4);
           
           }


    void Spawn ()
    {
       // if(playerHealth.currentHealth <= 0f)
        //{
          //  return;
        //}

        int spawnPointIndex = Random.Range (0, spawnPoints.Length);
        spawnPosition = new Vector3(Random.insideUnitSphere.x * radius,
                                   transform.position.y + Random.Range(-0.5f, 0.5f), Random.insideUnitSphere.z * radius);

        //Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        Instantiate (enemy, spawnPosition, spawnPoints[spawnPointIndex].rotation);
    }
}
