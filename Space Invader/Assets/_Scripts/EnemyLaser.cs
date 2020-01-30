using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public float speed = 10;
    public Transform enemy;
    public bool active;

    private GameObject game_manager;
    private Rigidbody rd;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        rd.velocity = transform.forward * speed;
        //we play the weapon on audio when the laser created
        gameObject.GetComponent<AudioSource>().Play();
        game_manager = GameObject.Find("GameManager");
        //register in game_manager's list
        game_manager.GetComponent<GameManager>().enemy_laser_list.Add(gameObject);
        //Physics.IgnoreCollision(GetComponent<Collider>(), enemy.GetComponent<Collider>());

        active = true;
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
        if (active)
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
            //after we reach the Earth, deactive it
            else if (collision.collider.tag == "Earth")
            {
                active = false;
            }
        }
    }

    void LaserDie()
    {
        Destroy(gameObject);
    }
}
