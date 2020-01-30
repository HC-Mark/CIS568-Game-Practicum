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
        if (other.tag == "Asteroid" || other.tag == "Asteroid2" || other.tag == "Asteroid3")
        {
            //update score here to game manager
            game_manager.GetComponent<GameManager>().AddScore(other.tag);
            //update the item count
            game_manager.GetComponent<GameManager>().AddItemCount(other.tag);
           //destroy correspond object
           other.GetComponent<Asteroid>().AsteroidDie();
            //need to clean up the left effect object ?
            //create explosion effect
            Instantiate(asteroid_explosion, transform.position, transform.rotation);
            LaserDie();
        }
        if (other.tag == "LargeEnemy" || other.tag == "SmallEnemy" || other.tag == "MediumEnemy")
        {
            if (other.GetComponent<EnemyControl>().active)
            {
                //update score here to game manager
                game_manager.GetComponent<GameManager>().AddScore(other.tag);
                //update score here to game manager
                //other.GetComponent<EnemyControl>().EnemyDie();
                other.GetComponent<EnemyControl>().active = false;
                other.GetComponent<Rigidbody>().useGravity = true;
                //turn off trigger, turn to collider
                //other.GetComponent<Rigidbody>().isKinematic = true;
                //need to clean up the left effect object ?
                //create explosion effect
                Instantiate(enemy_explosion, transform.position, transform.rotation);
                LaserDie();
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Asteroid" || collision.collider.tag == "Asteroid2" || collision.collider.tag == "Asteroid3")
        {
            //update score here to game manager
            game_manager.GetComponent<GameManager>().AddScore(collision.collider.tag);
            //destroy correspond object
            collision.collider.GetComponent<Asteroid>().AsteroidDie();
            //need to clean up the left effect object ?
            //create explosion effect
            Instantiate(asteroid_explosion, transform.position, transform.rotation);
            LaserDie();
        }
        if (collision.collider.tag == "LargeEnemy" || collision.collider.tag == "SmallEnemy" || collision.collider.tag == "MediumEnemy")
        {
            if (collision.gameObject.GetComponent<EnemyControl>().active)
            {
                //update score here to game manager
                game_manager.GetComponent<GameManager>().AddScore(collision.collider.tag);
                //update score here to game manager
                //no more directly die, we change turn on gravity and turn it into inactive status
                collision.gameObject.GetComponent<EnemyControl>().active = false;
                collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
                //turn off trigger, turn to collider
                collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                //collision.collider.GetComponent<EnemyControl>().EnemyDie();
                //need to clean up the left effect object ?
                //create explosion effect
                Instantiate(enemy_explosion, transform.position, transform.rotation);
                LaserDie();
            }
        }
    }

    void LaserDie()
    {
        if(player_ship != null)    player_ship.GetComponent<PlayerControl>().fire_peroid = player_ship.GetComponent<PlayerControl>().fire_rate; //able to fire again
        Destroy(gameObject);
    }
}
