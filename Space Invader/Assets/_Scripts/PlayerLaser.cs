using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    public float speed = 2;
    public GameObject player_ship;
    public GameObject asteroid_explosion;
    public GameObject enemy_explosion;

    private GameObject game_manager;
    private Rigidbody rd;
    // Start is called before the first frame update
    void Start()
    {

        rd = GetComponent<Rigidbody>();
        game_manager = GameObject.Find("GameManager");
        //gameObject.transform.Translate(0.0f, 0.0f, speed);
        rd.velocity = transform.forward * speed;

        //doesn't work if I directly assign the prefab to the laser object, why?
        player_ship = GameObject.Find("Player Variant(Clone)");
        //we play the audio when the laser created
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
        if (other.tag == "Asteroid")
        {
            //update score here to game manager
            other.GetComponent<Asteroid>().AsteroidDie();
            //need to clean up the left effect object ?
            //create explosion effect
            Instantiate(asteroid_explosion, transform.position, transform.rotation);
            LaserDie();
        }
    }

    void LaserDie()
    {
        player_ship.GetComponent<PlayerControl>().is_shot = false;
        Destroy(gameObject);
    }
}
