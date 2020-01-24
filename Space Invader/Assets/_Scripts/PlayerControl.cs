using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//customized class must be able to serialize to the main gameobject
[System.Serializable]
public class Boundary
{
    public float x_min, x_max;
}
public class PlayerControl : MonoBehaviour
{
    //public Variables
    public float move_speed = 0.3f;
    public float revive_time = 3.0f;
    public Boundary boundary;
    public float tilt;
    public bool is_shot = false; //determine whether the player's ship has shot, if so, can't shoot until the bullet destroyed

    //related gameobject -- need to be initialized in editor
    public GameObject player_laser;
    public GameObject player_explosion;
    public Transform player_laser_spawn;


    //private
    GameObject game_manager;
    // Start is called before the first frame update
    void Start()
    {
        //initialize game_manager
        game_manager = GameObject.Find("GameManager");
    }

    //fixed update to update the movement of ship
    private void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            gameObject.transform.Translate(move_speed, 0, 0);
            //we also want to have a tilt effect -- tilt should be reverse the movement direction to 
            gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -tilt);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            gameObject.transform.Translate(-move_speed, 0, 0);
            gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, tilt);
        }
        else if (Input.GetAxisRaw("Horizontal") == 0)
        {
            //reset the rotation
            gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }

        //can't exceed the boundary
        //we need only limit the x-axis because player can only move left to right
        float x_pos = Mathf.Clamp(gameObject.transform.position.x, boundary.x_min, boundary.x_max);
        gameObject.transform.position = new Vector3(x_pos, 0.0f, gameObject.transform.position.z);

    }


    // Update is called once per frame
    void Update()
    {
        //shoot the laser if we press space
        if (Input.GetButton("Fire1") && !is_shot)
        {
            Debug.Log("fire");
            Instantiate(player_laser, player_laser_spawn.position, player_laser.transform.rotation);
            is_shot = true;
            game_manager.GetComponent<GameManager>().AddScore("test");
        }
        if (game_manager.GetComponent<GameManager>().die)
        {
            game_manager.GetComponent<GameManager>().die = false;
            PlayerDie();
            
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyLaser" || other.tag == "Enemy")
        {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        Instantiate(player_explosion, transform.position, transform.rotation);
        //update the life information in game manger
        game_manager.GetComponent<GameManager>().LoseLife();
        game_manager.GetComponent<GameManager>().player_on_scene = false;
        Destroy(gameObject);
    }
}
