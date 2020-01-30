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
    //no longer use is_shot, use fire_rate
    //fire system
    public float fire_peroid;
    public float fire_rate;
    public bool is_shot = false; //determine whether the player's ship has shot, if so, can't shoot until the bullet destroyed

    //related gameobject -- need to be initialized in editor
    public GameObject player_laser;
    public GameObject player_explosion;
    public Transform player_laser_spawn;


    //private
    GameObject game_manager;
    Rigidbody cur_rigid;

    private bool fired;
    private Quaternion ori_rot;
    // Start is called before the first frame update
    void Start()
    {
        //initialize game_manager
        game_manager = GameObject.Find("GameManager");
        cur_rigid = gameObject.GetComponent<Rigidbody>();
        ori_rot = transform.rotation;
        fire_peroid = 0;
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

        //always reset Y value to zero and velocity on z to zero
        if (transform.position.y != 0 || transform.position.z != 0.5)
        {
            Vector3 temp_pos = transform.position;
            temp_pos.z = 0.5f;
            temp_pos.y = 0;
            transform.position = temp_pos;
        }

        if (transform.rotation.x != 0 || transform.rotation.y != 0 || transform.rotation.z != 0)
        {
            transform.rotation = ori_rot;
        }
        if (cur_rigid.velocity.z != 0 || cur_rigid.velocity.y != 0 || cur_rigid.velocity.x != 0)
        {
            Vector3 temp_vel = cur_rigid.velocity;
            temp_vel.x = 0;
            temp_vel.z = 0;
            temp_vel.y = 0;
            cur_rigid.velocity = temp_vel;
        }

    }


    // Update is called once per frame
    void Update()
    {
        //shoot the laser if we press space
        if (Input.GetButton("Fire1") && !fired)
        {
            //Debug.Log("fire");
            Instantiate(player_laser, player_laser_spawn.position, player_laser.transform.rotation);
            fired = true;
            //reset fire_period
            fire_peroid = 0;
            //game_manager.GetComponent<GameManager>().AddScore("test");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("switch camera");
        }
        if (game_manager.GetComponent<GameManager>().die)
        {
            game_manager.GetComponent<GameManager>().die = false;
            PlayerDie();
            
        }

        if (fired)
        {
            fire_peroid += Time.deltaTime;
            if (fire_peroid >= fire_rate) fired = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyLaser" || other.tag == "Enemy")
        {
            PlayerDie();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "EnemyLaser")
        {
            if(collision.gameObject.GetComponent<EnemyLaser>().active)   PlayerDie();
        }

        if (collision.collider.tag == "LargeEnemy" || collision.collider.tag == "SmallEnemy" || collision.collider.tag == "MediumEnemy")
        {
            if (collision.gameObject.GetComponent<EnemyControl>().active)
            {
                PlayerDie();
            }
        }

    }

    public void PlayerDie()
    {
        Instantiate(player_explosion, transform.position, transform.rotation);
        //update the life information in game manger
        game_manager.GetComponent<GameManager>().LoseLife();
        game_manager.GetComponent<GameManager>().player_on_scene = false;
        Destroy(gameObject);
    }
}
