using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public GameObject shot;
    private bool right;
    public float shootFrequency;
    private Vector3 endPos;
    private float startTime;
    private bool down = false;
    public float downSpeed = 5f;
    public float downLength = 0.5f;
    public float tilt;
    private Rigidbody[] rigidbodies;
    private Quaternion target;
    public float smooth;
    public Transform bonusSpawn;

    public AudioSource enemyExplosion;
    public AudioSource asteriodExplosion;
    public AudioSource enemyShoot;
    public GameObject enemyExploAnimation;

    private bool speedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        right = true;
        GetComponent<Rigidbody>().velocity = new Vector3(1.0f, 0f, 0f) * speed;

    }

    void Move()
    {   
        if(down == true && transform.position.z < endPos.z)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(1.0f, 0f, 0f) * speed;
            down = false;
        }


        foreach (Transform enemy in transform)
        {
            if (enemy.position.x < -6 && right == false)
            {
                speed = -speed;
                right = true;
                startTime = Time.time;             
                down = true;
                endPos = transform.position - downLength * Vector3.forward;
                GetComponent<Rigidbody>().velocity = new Vector3(speed, 0f, -downSpeed);
                target = Quaternion.Euler(enemy.rotation.x, enemy.rotation.y, enemy.rotation.z + 30f);
            }
            if(enemy.position.x > 6 && right == true)
            {
                speed = -speed;
                right = false;
                startTime = Time.time;
                down = true;
                endPos = transform.position - downLength * Vector3.forward;
                GetComponent<Rigidbody>().velocity = new Vector3(speed, 0f, -downSpeed);
                target = Quaternion.Euler(enemy.rotation.x, enemy.rotation.y, enemy.rotation.z - 30f);
            }
            //enemy.rotation = Quaternion.Slerp(enemy.rotation, target, Time.deltaTime * smooth);


            if (enemy.position.z < -6)
            {
                GameController.playerDead = true;
                Time.timeScale = 0;
            }
        }

        if(!speedUp && transform.childCount <= 6)
        {
            speedUp = true;
            speed *= 2;
        }

        if(transform.childCount == 0)
        {
            //GameController gameController = GameObject.Find("/GameController").GetComponent<GameController>();
            //gameController.gameOver();
            GameController.win = true;
        }
    }

    
    void shoot()
    {
        foreach (Transform enemy in transform)
        {
            Transform shootSpawn = enemy.Find("Shot Spawn").gameObject.transform;

            RaycastHit hit;

            if (Random.Range(0f, 1f) < shootFrequency / 200f
                && (Physics.Raycast(enemy.position, -Vector3.forward, out hit) && hit.transform.tag != "Enemy"
                    || !Physics.Raycast(enemy.position, -Vector3.forward, out hit)))
            {
                Instantiate(shot, shootSpawn.position, shootSpawn.rotation);
                enemyShoot.Play(0);
            }

        }
    }

    public float nextBounsTime = 10f;
    private float nextBouns = 0f;
    public GameObject bonus;
    private GameObject bonusObj;
    private void Update()
    {
        if(Time.time > nextBouns)
        {
            if(bonusObj!= null)
            {
                Destroy(bonusObj);
            }
            bonusObj = Instantiate(bonus, bonusSpawn.position, bonusSpawn.rotation);
            bonusObj.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 10f;
            bonusObj.GetComponent<Rigidbody>().velocity = Vector3.left * 3f;
            nextBouns = Time.time + nextBounsTime;
        }

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
        shoot();
    }
}
