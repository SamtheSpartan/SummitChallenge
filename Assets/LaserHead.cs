using UnityEngine;
using System.Collections;

public class LaserHead : MonoBehaviour {
    public ParticleSystem shotParticles;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.PageUp) || Input.GetKeyDown(KeyCode.PageDown))
        {
            shotParticles.Play();
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if(hit.collider.gameObject.tag == "Enemy")
                {
                    //Do enemy logic here
                }
            }
        }
	}
    void Shoot()
    {

    }
}
