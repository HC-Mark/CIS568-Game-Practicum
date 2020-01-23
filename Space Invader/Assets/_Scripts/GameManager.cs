using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AsteroidSpawn
{
    public float asteroid_wait_min, asteroid_wait_max;
}

public class GameManager : MonoBehaviour
{
    public GameObject asteroid;
    public GameObject enemy;
    public Vector3 asteroid_spawn_value;
    public bool asteroid_on_scene;
    public int enemy_num;
    //corountine setup
    public AsteroidSpawn asteroid_spawn;

    // Start is called before the first frame update
    void Start()
    {
        //we need to set up coroutine to spawn it infintely
       
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: switch to based on probability
        if (!asteroid_on_scene)
        {
            StartCoroutine(SpawnAsteroids());
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
}
