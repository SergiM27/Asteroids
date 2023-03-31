using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuController : MonoBehaviour
{

    public int livesToRestart;
    private TextMeshProUGUI scoreText;
    private GameObject highScoreText;

    private void Start()
    {
        SetTexts();
    }

    private void SetTexts()
    {
        highScoreText = GameObject.Find("NewHighscore");
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        scoreText.text = "Your score was:     " + GameManager.instance.currentScore.ToString();

        if (GameManager.instance.newHighScore)
        {
            highScoreText.SetActive(true);
        }
        else
        {
            highScoreText.SetActive(false);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance.PlaySFX("Play");
            RestartVariables();
            SceneManager.LoadScene("Game");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void RestartVariables()
    {
        GameManager.instance.lives = livesToRestart; 
    }
}
