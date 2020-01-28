using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.tag == "Shield")
        {
            GameController.playerDead = true;
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
