using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject objToSpawn;
    public float timer;
    public float spawnPeriod;
    public int numberSpawnedEachPeriod;
    public Vector3 originInScreenCoords;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        //initialize
        score = 0;
        timer = 0;
        spawnPeriod = 5.0f;
        numberSpawnedEachPeriod = 3;
        originInScreenCoords = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0)); //why it works?
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnPeriod)
        {
            timer = 0;
            float width = Screen.width;
            float height = Screen.height;
            for (int i = 0; i < numberSpawnedEachPeriod; i++)
            {
                float horizontalPos = Random.Range(0.0f, width);
                float verticalPos = Random.Range(0.0f, height);
                Instantiate(objToSpawn,Camera.main.ScreenToWorldPoint(new Vector3(horizontalPos,verticalPos, originInScreenCoords.z)), Quaternion.identity);
            }

            //test
            //because we store the origin of screen coordinate, and we only concern two dimension, z is actually the depth of the origin.
            //Therefore, we can make sure that the four asteroids we create, will share the same depth in the screen space, which means that they will all be 0 on the Y-axis, because that is the Depth for the game.

            //Vector3 botLeft = new Vector3(0, 0, originInScreenCoords.z);
            //Vector3 botRight = new Vector3(width, 0,
            //originInScreenCoords.z);
            //Vector3 topLeft = new Vector3(0, height,
            //originInScreenCoords.z);
            //Vector3 topRight = new Vector3(width, height,
            //originInScreenCoords.z);
            //Instantiate(objToSpawn, Camera.main.ScreenToWorldPoint(topLeft), Quaternion.identity);
            //Instantiate(objToSpawn, Camera.main.ScreenToWorldPoint(topRight), Quaternion.identity);
            //Instantiate(objToSpawn, Camera.main.ScreenToWorldPoint(botLeft), Quaternion.identity);
            //Instantiate(objToSpawn, Camera.main.ScreenToWorldPoint(botRight), Quaternion.identity);
        }

        //quit the game by pressing escape
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
