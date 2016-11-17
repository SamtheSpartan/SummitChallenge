using UnityEngine;
using System.Collections;

public class LaserHead : MonoBehaviour {
    public ParticleSystem shotParticles;
    public GameObject[] startButtonObjects;
    public CarController car;
    public EnemyManager spawner;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.PageUp) || Input.GetKeyDown(KeyCode.PageDown))
        {
            Shoot();
        }
	}
    void Shoot()
    {
        shotParticles.Play();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                //Do enemy logic here
                hit.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(50, hit.point);
            }
            if (hit.collider.gameObject.tag == "Start")
            {
                for (int i = 0; i < startButtonObjects.Length; i++)
                {
                    startButtonObjects[i].SetActive(false);
                    car.navState = CarController.NavState.PATHFINDING;
                }
                spawner.canSpawn = true;
            }
        }
    }
}
