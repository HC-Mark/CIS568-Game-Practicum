using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class AsteroidSpawn
{
    public float asteroid_wait_min, asteroid_wait_max;
}

class EnemyScore
{
    public int small = 30;
    public int medium = 20;
    public int large = 10;
}

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject asteroid;
    public GameObject enemy;
    public Transform camera_transform;

    public Text life_text;
    public Text score_text;
    public Text gameover_text;
    public Text restart_text;

    public Vector3 asteroid_spawn_value;
    public Vector3 player_spawn_pos;
    public bool asteroid_on_scene;
    public bool player_on_scene;
    public int enemy_num;


    //corountine setup
    public AsteroidSpawn asteroid_spawn;
    public float player_spawn; //or we use it as the shake duration when player die
    //test
    public bool die;

    //private variables -- GUI
    private int score;
    private int life_remain;
    private EnemyScore es;
    private int min_asteroid_score;
    private int max_asteroid_score;
    private bool game_over;
    private bool restart;
    //for shake effect
    private float shake_amount = 0.7f;
    private Vector3 original_position;
    private bool shake_effect_on;
    private float shake_duration;
    // Start is called before the first frame update
    void Start()
    {
        if (camera_transform == null)
        {
            //find it
            camera_transform = GameObject.Find("Main Camera").transform;
        }
        shake_effect_on = false;
        //we need to set up coroutine to spawn it infintely
        player_on_scene = true;

        //no delay when create the game
        StartCoroutine(SpawnPlayer(0));

        //initialize score and life_remain;
        score = 0;
        life_remain = 3;
        min_asteroid_score = 30;
        max_asteroid_score = 100;
        UpdateLife();
        UpdateScore();

        //initialize and hide the gameover text and restart text
        game_over = false;
        restart = false;
        gameover_text.gameObject.SetActive(false);
        restart_text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {


        //TODO: switch to based on probability
        if (game_over)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Main");
            }
        }
        else
        {
            if (!asteroid_on_scene)
            {
                StartCoroutine(SpawnAsteroids());
            }
            if (!player_on_scene)
            {
                StartCoroutine(SpawnPlayer(player_spawn));
            }
        }

        if (shake_effect_on)
        {
            ShakeEffect();
        }
    }

    IEnumerator SpawnAsteroids() {
        asteroid_on_scene = true;
        float asteroid_wait = Random.Range(asteroid_spawn.asteroid_wait_min, asteroid_spawn.asteroid_wait_max);
        yield return new WaitForSeconds(asteroid_wait);
        Vector3 spawn_position = asteroid_spawn_value;
        Quaternion spawn_rotation = Quaternion.identity;
        Instantiate(asteroid, spawn_position, spawn_rotation);
    }

    IEnumerator SpawnPlayer(float spawn_time)
    {
        //detect whether the life_remain is less than 0
        if (life_remain >= 0)
        {
            player_on_scene = true;
            yield return new WaitForSeconds(spawn_time);
            Vector3 spawn_position = player_spawn_pos;
            Quaternion spawn_rotation = Quaternion.identity;
            Instantiate(player, spawn_position, spawn_rotation);
        }
        else
        {
            GameOver();
        }
    }

    public void AddScore(string tag)
    {
        if (tag == "test")
        {
            score++;
        }
        else if (tag == "small")
        {
            score += es.small;
        }
        else if (tag == "medium")
        {
            score += es.medium;
        }
        else if (tag == "small")
        {
            score += es.large;
        }
        else if (tag == "Asteroid")
        {
            //we have a random value between two values
            score += Random.Range(min_asteroid_score, max_asteroid_score);
        }
        UpdateScore();
    }

    void UpdateScore()
    {
        score_text.text = "Score:" + score;
    }

    void UpdateLife()
    {
        int life_remain_text;
        if (life_remain < 0)
        {
            life_remain_text = 0;
        }
        else
        {
            life_remain_text = life_remain;
        }
        life_text.text = "Life:" + life_remain_text;
    }

    public void GameOver()
    {
        game_over = true;
        gameover_text.gameObject.SetActive(true);
        restart_text.gameObject.SetActive(true);
    }

    public void LoseLife()
    {
        life_remain--;
        UpdateLife();
        InitShakeEffect();
        Debug.Log("lose life");
    }

    //camera shake -- get inspiration from https://gist.github.com/ftvs/5822103
    void InitShakeEffect()
    {
        original_position = camera_transform.localPosition;
        shake_effect_on = true;
        shake_duration = player_spawn;
    }

    void ShakeEffect()
    {
        camera_transform.localPosition = original_position + Random.insideUnitSphere * shake_amount;
        shake_duration -= Time.deltaTime;
        if(shake_duration < 0)
        {
            shake_effect_on = false;
            //reset the position of the camera
            camera_transform.localPosition = original_position;
        }
    }

}
