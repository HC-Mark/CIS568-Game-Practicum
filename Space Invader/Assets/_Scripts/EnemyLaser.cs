using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public float speed = 10;
    public Transform enemy;

    private GameObject game_manager;
    private Rigidbody rd;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        rd.velocity = transform.forward * speed;
        //we play the weapon on audio when the laser created
        gameObject.GetComponent<AudioSource>().Play();

        Physics.IgnoreCollision(GetComponent<Collider>(), enemy.GetComponent<Collider>());
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

    private void OnCollisionEnter(Collision collision)
    {
        //we handle the player info update in player control script
        if (collision.collider.tag == "Player")
        {
            LaserDie();
        }
        else if (collision.collider.tag == "LargeEnemy" || collision.collider.tag == "SmallEnemy" || collision.collider.tag == "MediumEnemy")
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
        }
    }

    void LaserDie()
    {
        Destroy(gameObject);
    }
}
