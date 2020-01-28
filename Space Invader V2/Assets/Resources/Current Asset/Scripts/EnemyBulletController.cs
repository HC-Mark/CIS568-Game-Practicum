using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public GameController gameController;
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        gameController = GameObject.Find("/GameController").GetComponent<GameController>();
    }

    private void FixedUpdate()
    {
        
    }
    private GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameController.playerdie.Play(0);
            player = GameObject.Find("/Player").gameObject;
            Instantiate(player.GetComponent<PlayerController>().playerExplosion, player.transform.position, player.transform.rotation);
            Destroy(other.gameObject);
            Destroy(gameObject);
            GameController.playerDead = true;
        } else if(other.tag == "Shield")
        {
            //gameController.shieldAttcked.Play(0);
            other.gameObject.GetComponent<shieldLife>().ifGetShot();
            Destroy(gameObject);
            shieldLife shield = other.gameObject.GetComponent<shieldLife>();
            shield.life -= 1;
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < -10)
        {
            Destroy(gameObject);
        }
    }
}
