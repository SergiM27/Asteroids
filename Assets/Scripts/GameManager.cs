using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player and Lives")]
    public PlayerController player;
    public int lives = 3;
    public GameObject[] livesUI;

    [Header("Respawn")]
    public float timeBeforeRespawn;
    public float respawnInmunityTime;

    [Header("Score")]
    private ScoreController scoreController;
    public bool newHighScore; //Used to know whether in the GameOver screen "New Highscore" should be shown or not.
    public int currentScore;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (!PlayerPrefs.HasKey("HighScore")) PlayerPrefs.SetInt("HighScore", 0);
    }



    public void SetPlayer(PlayerController newPlayer)
    {
        player = newPlayer;
    }

    public void SetLives()
    {
        livesUI[0] = GameObject.Find("Life3");
        livesUI[1] = GameObject.Find("Life2");
        livesUI[2] = GameObject.Find("Life1");
    }

    public void SetScoreController(ScoreController _scoreController)
    {
        scoreController = _scoreController;
    }


    public void PlayerDeath()
    {
        lives--;
        if (lives == 0)
        {
            GameOver();
            AudioManager.instance.PlaySFX2("GameOver");
            scoreController.SetScoreGameOver();
        }
        else
        {
            StartCoroutine("Respawn");
            AudioManager.instance.PlaySFX2("Death");
        }
        UILivesUpdate();
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void UILivesUpdate()
    {
        switch (lives)
        {
            case 3:
                livesUI[0].SetActive(true);
                livesUI[1].SetActive(true);
                livesUI[2].SetActive(true);
                break;
            case 2:
                livesUI[0].SetActive(false);
                livesUI[1].SetActive(true);
                livesUI[2].SetActive(true);
                break;
            case 1:
                livesUI[0].SetActive(false);
                livesUI[1].SetActive(false);
                livesUI[2].SetActive(true);
                break;
            case 0:
                livesUI[0].SetActive(false);
                livesUI[1].SetActive(false);
                livesUI[2].SetActive(false);
                break;
        }
    }

    public void SetHighScore(int score)
    {
        if (PlayerPrefs.GetInt("HighScore") < score)
        {
            PlayerPrefs.SetInt("HighScore", score);
            newHighScore = true;
        }
        else
        {
            newHighScore = false;
        }
        currentScore = score;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(timeBeforeRespawn);
        player.transform.position = Vector3.zero;
        player.gameObject.layer = LayerMask.NameToLayer("PlayerDead"); //This is applied so the player doesn't hit an asteroid when spawned instantly (avoid unfairness).
        player.gameObject.SetActive(true);
        yield return new WaitForSeconds(respawnInmunityTime);
        player.gameObject.layer = LayerMask.NameToLayer("Player"); //After respawnInmunityTime, player can again hit an asteroid.
    }
}
