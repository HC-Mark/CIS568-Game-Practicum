using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static bool playerDead = false;
    public static bool win = false;
    public Text scoreText;
    public static float score = 0;
    public AudioSource playerdie;
    public Text wintext;
    public Text lossText;

    
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.Find("/Canvas/ScoreText").GetComponent<Text>();
        
    }



    public void reStartGame()
    {
        GameController.playerDead = false;
        score = 0;
        Time.timeScale = 1;

        SceneManager.LoadScene("PlayScene");
        win = false;
        wintext.gameObject.SetActive(false);
        lossText.gameObject.SetActive(false);

    }

    bool waitActive = false; //so wait function wouldn't be called many times per frame

    IEnumerator WaitLoss()
    {
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = 0;
    }

    IEnumerator WaitWin()
    {
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = 0;
    }
    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        if(playerDead == true)
        {
            lossText.gameObject.SetActive(true);
            if (!waitActive)
            {
                StartCoroutine(WaitLoss());
            }
            
        }
        if(win)
        {
            wintext.gameObject.SetActive(true);
            StartCoroutine(WaitWin());
        }

    }
}
