using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float tumble;
    public Vector2 asteroid_speed;

    private GameObject game_manager;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody cur_rd = gameObject.GetComponent<Rigidbody>();
        game_manager = GameObject.Find("GameManager");
        cur_rd.angularVelocity = Random.insideUnitSphere * tumble;
        float x_speed = Random.Range(asteroid_speed.x, asteroid_speed.y);
        cur_rd.velocity = new Vector3(-x_speed, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //if it leaves the boundary destroy
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boundary")
        {
            AsteroidDie();
        }
    }

    public void AsteroidDie()
    {
        game_manager.GetComponent<GameManager>().asteroid_on_scene = false;
        Destroy(gameObject);
    }
}
