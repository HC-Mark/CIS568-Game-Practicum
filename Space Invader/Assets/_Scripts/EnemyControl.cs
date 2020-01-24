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

    //private
    GameObject game_manager;
    private MoveStandard move_standard;
    private MoveController move_controller;

    // Start is called before the first frame update
    void Start()
    {
        //find game manager
        game_manager = GameObject.Find("GameManager");
        //initialize helper class
        move_standard = new MoveStandard();
        move_controller = new MoveController();
        StartCoroutine(Movement());
        StartCoroutine(Fire());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Movement()
    {
        while (true)
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
            yield return new WaitForSeconds(fire_rate);
            Instantiate(laser, laser_spawn_pos.position, laser_spawn_pos.rotation);
            //is_shot = true;
        }
    }
}
