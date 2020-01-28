using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldLife : MonoBehaviour
{
    public int life;
    public AudioSource explosion;
    private Vector3 originPos;
    private GameObject explosionAnimation;

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        explosionAnimation = GameObject.Find("/Enemies").GetComponent<EnemyController>().enemyExploAnimation;
    }

    // Update is called once per frame
    private bool ifShot = false;
    private float shakeTime;
    public void ifGetShot()
    {
        ifShot = true;
        shakeTime = Time.time + 0.4f;
    }
    void Update()
    {
        if(!ifShot)
        {
            transform.position = new Vector3(originPos.x + Mathf.Sin(Time.time * 1f) * 0.2f, originPos.y, originPos.z + Mathf.Sin(Time.time * 0.8f) * 0.2f);
        }else
        {
            transform.position = Random.insideUnitSphere * 0.02f + transform.position;
        }
           

        if (life < 1)
        {
            Instantiate(explosionAnimation, transform.position, transform.rotation);
            explosion.Play(0);
            Destroy(gameObject);

        }
        
        if (Time.time > shakeTime)
        {
            ifShot = false;
            
        }


    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            
            Instantiate(explosionAnimation, transform.position, transform.rotation);
            explosion.Play(0);

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }


}
