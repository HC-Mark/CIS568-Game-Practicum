using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//how much time between each move
//how long should enemy move
class MoveStandard {
    public float interval = 2.5f;
    public float horizontal_constant = 0.5f;
    public float vertical_constant = 1;
    public int half_way = 4;
    public int whole_way = 8;
}

class MoveController {
    public bool first_round = true;
    public bool toward_left = true;
    public bool toward_right = false;
}

public class EnemyControl : MonoBehaviour
{

    //public
    public GameObject laser;
    public Transform laser_spawn_pos;
    public float fire_rate_min;
    public float fire_rate_max;
    public float delay;
    public bool active;

    //private
    private bool fired;
    private Vector3 fwd;
    private float dist;
    GameObject game_manager;
    private Rigidbody cur_rigid;
    private MoveStandard move_standard;
    private MoveController move_controller;
    private IEnumerator moving;
    private IEnumerator firing;
    // Start is called before the first frame update
    void Start()
    {
        //find game manager
        game_manager = GameObject.Find("GameManager");
        cur_rigid = gameObject.GetComponent<Rigidbody>();
        active = true;

        //https://gamedev.stackexchange.com/questions/97429/stopcoroutine-is-not-stopping-my-coroutine-in-unity
        //we need to store the IEnumerator and directly refer to that specific corountine, otherwise we can't stop the coroutine
        moving = Movement();
        firing = Fire();

        //initialize helper class
        move_standard = new MoveStandard();
        move_controller = new MoveController();
        StartCoroutine(moving);
        //for fire system
        fired = false;
        dist = 5;
        fwd = transform.TransformDirection(Vector3.forward);



    }

    // Update is called once per frame
    void Update()
    {
        //reset the position if its y value and velocity is wrong
        if (transform.position.y != 0)
        {
            Vector3 temp_pos = transform.position;
            temp_pos.y = 0;
            transform.position = temp_pos;
        }

        if (cur_rigid.velocity.y != 0)
        {
            Vector3 temp_vel = cur_rigid.velocity;
            temp_vel.y = 0;
            cur_rigid.velocity = temp_vel;
        }

        if (!fired)
        {
            if (!Physics.Raycast(transform.position, fwd, dist))
            {
                StartCoroutine(firing);
                fired = true;
            }
        }

        if (!active)
        {
            //step firing
            StopCoroutine(firing);
            StopCoroutine(moving);
        }
    }

    IEnumerator Movement()
    {
        while (active)
        {
            if (move_controller.first_round == true)
            {
                for (int move_step = 0; move_step <= move_standard.half_way; ++move_step)
                {
                    yield return new WaitForSeconds(move_standard.interval);
                    //get the position
                    Vector3 cur_pos = gameObject.transform.position;
                    //move horizontally
                    if (move_step != move_standard.half_way)
                    {
                        if (move_controller.toward_left == true)
                        {
                            //based on it self
                            cur_pos.x -= move_standard.horizontal_constant;
                            gameObject.transform.position = cur_pos;

                        }
                        else if (move_controller.toward_right == true)
                        {
                            cur_pos.x += move_standard.horizontal_constant;
                            gameObject.transform.position = cur_pos;
                        }
                    }
                    //move vertically and update move controller
                    else
                    {
                        move_controller.first_round = false;
                        move_controller.toward_left = !move_controller.toward_left;
                        move_controller.toward_right = !move_controller.toward_right;
                        cur_pos.z -= move_standard.vertical_constant;
                        gameObject.transform.position = cur_pos;
                    }
                }
            }
            else {
                for (int move_step = 0; move_step <= move_standard.whole_way; ++move_step)
                {
                    yield return new WaitForSeconds(move_standard.interval);
                    Vector3 cur_pos = gameObject.transform.position;
                    //move horizontally
                    if (move_step != move_standard.whole_way)
                    {
                        if (move_controller.toward_left == true)
                        {
                            cur_pos.x -= move_standard.horizontal_constant;
                            gameObject.transform.position = cur_pos;
                        }
                        else if (move_controller.toward_right == true)
                        {
                            cur_pos.x += move_standard.horizontal_constant;
                            gameObject.transform.position = cur_pos;
                        }
                    }
                    //move vertically and update move controller
                    else
                    {
                        //switch to the other direction
                        move_controller.toward_left = !move_controller.toward_left;
                        move_controller.toward_right = !move_controller.toward_right;
                        cur_pos.z -= move_standard.vertical_constant;
                        gameObject.transform.position = cur_pos;
                    }
                }
            }


            //reset all the velocity, only moved by translation
            if (cur_rigid.velocity.y != 0 || cur_rigid.velocity.x != 0 || cur_rigid.velocity.z != 0)
            {
                cur_rigid.velocity = new Vector3(0,0,0);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boundary")
        {
            EnemyDie();
        }
    }
    //die
    public void EnemyDie()
    {
        //send info to game manager
        game_manager.GetComponent<GameManager>().EnemyDie();
        //laser send score to GameManager
        Destroy(gameObject);
    }

    IEnumerator Fire()
    {
        while (true)
        {
                float fire_rate = Random.Range(fire_rate_min, fire_rate_max);
                //test
                //float fire_rate = 1;
                yield return new WaitForSeconds(fire_rate);
                GameObject cur_laser = Instantiate(laser, laser_spawn_pos.position, laser_spawn_pos.rotation);
                cur_laser.GetComponent<EnemyLaser>().enemy = gameObject.transform;
        }
    }
}
