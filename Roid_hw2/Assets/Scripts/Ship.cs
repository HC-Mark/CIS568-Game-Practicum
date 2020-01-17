using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the name of the class should be consistent with the script name
public class Ship : MonoBehaviour
{
    // some public variables that can be used to tune the Ship’s movement
    public Vector3 forceVector;
    public float rotationSpeed;
    public float rotation;

    public GameObject bullet; // the GameObject to spawn
    // Use this for initialization 
    void Start () {
        forceVector.x = 1.0f;
        rotationSpeed = 2.0f;
    } // Update is called once per frame 

    //should be applied on any thing that is continuously affect the object, like physical controls
    private void FixedUpdate()
    {
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            GetComponent<Rigidbody>().AddRelativeForce(forceVector);
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            GetComponent<Rigidbody>().AddRelativeForce(-forceVector);
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            rotation += rotationSpeed;

        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            rotation -= rotationSpeed;
        }
        Quaternion rot = Quaternion.Euler(new Vector3(0, rotation, 0));
        GetComponent<Rigidbody>().MoveRotation(rot); //use quaternion
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Fire! " + rotation);
            /* we don’t want to spawn a Bullet inside our ship, so some
            Simple trigonometry is done here to spawn the bullet at the tip of where the ship is pointed.
            */
            Vector3 spawnPos = gameObject.transform.position;
            spawnPos.x += 1.5f * Mathf.Cos(rotation * Mathf.PI / 180);
            spawnPos.z -= 1.5f * Mathf.Sin(rotation * Mathf.PI / 180);
            // instantiate the Bullet
            GameObject obj = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject; //we rotate after we instantiate
            // get the Bullet Script Component of the new Bullet instance
            Bullet b = obj.GetComponent<Bullet>();
            // set the direction the Bullet will travel in
            Quaternion rot = Quaternion.Euler(new Vector3(0, rotation, 0));
            b.heading = rot;
        }
    }
}
