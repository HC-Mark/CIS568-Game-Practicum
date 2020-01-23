using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    public float speed = 2;
    public GameObject player_ship;

    private Rigidbody rd;
    // Start is called before the first frame update
    void Start()
    {

        rd = GetComponent<Rigidbody>();
        //gameObject.transform.Translate(0.0f, 0.0f, speed);
        rd.velocity = transform.forward * speed;

        player_ship = GameObject.Find("Player");
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boundary")
        {
            Die();
        }
    }

    void Die()
    {
        player_ship.GetComponent<PlayerControl>().is_shot = false;
        Destroy(gameObject);
    }
}
