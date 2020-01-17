using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    private Text scoreText;
    GameManager globalManager;
    // Use this for initialization

    string prefix = "Score:";
    void Start()
    {
        scoreText = GetComponent<Text>();
        GameObject g = GameObject.Find("GlobalManager");
        globalManager = g.GetComponent<GameManager>();

        //scoreText.text = "1";
    }
    // Update is called once per frame
    void Update()
    {
        //scoreText.GetComponent<UnityEngine.UI.Text>().text = prefix + globalManager.score.ToString();
        //scoreText.text = "1";
    }
}
