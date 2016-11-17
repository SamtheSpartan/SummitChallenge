using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //public PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;
    public float radius = 0;

    private Vector3 spawnPosition;
    private Vector3 direction;
    Transform player;
    public bool canSpawn;

    void Start ()
    {
        
        radius = Random.Range(2, 4);
        player = GameObject.FindGameObjectWithTag("Car").transform;

    }
    void Update()
    {
        if (canSpawn)
        {
            InvokeRepeating("Spawn", spawnTime, spawnTime);
            canSpawn = false;
        }
    }

    void Spawn ()
    {
       // if(playerHealth.currentHealth <= 0f)
        //{
          //  return;
        //}
        
        spawnPosition = new Vector3(Random.insideUnitSphere.x * radius,
                                   transform.position.y + Random.Range(-0.5f, 0.5f), Random.insideUnitSphere.z * radius);

        direction = player.position - transform.position;
        //Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        Instantiate (enemy, spawnPosition, Quaternion.LookRotation(direction));
    }
}
