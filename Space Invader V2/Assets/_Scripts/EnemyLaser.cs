using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public float speed = 10;

    private GameObject game_manager;
    private Rigidbody rd;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        rd.velocity = transform.forward * speed;
        //we play the weapon on audio when the laser created
        gameObject.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boundary")
        {
            LaserDie();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //we handle the player info update in player control script
        if (other.tag == "Player")
        {
            LaserDie();
        }
    }

    void LaserDie()
    {
        Destroy(gameObject);
    }
}
