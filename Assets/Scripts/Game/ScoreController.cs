using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public int score;
    public int scoreToNextSpawnTime, currentScoreToNextSpawnTime, scoreToNextAmount, currentScoreToNextAmount;

    private ObjectSpawner objectSpawner;

    private void Start()
    {
        GameManager.instance.SetScoreController(this);
        objectSpawner = FindObjectOfType<ObjectSpawner>();
        SetHighScoreText();
    }

    private void SetHighScoreText()
    {
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore");
    }

    public void AddScore(int amount)
    {
        score += amount;
        currentScoreToNextSpawnTime += amount;
        currentScoreToNextAmount += amount;
        scoreText.text = "Score: " + score.ToString();
        CheckSpawnLevel();
        CheckAmountLevel();
    }

    private void CheckSpawnLevel() //Used to change spawn time based on current score
    {
        if (currentScoreToNextSpawnTime >= scoreToNextSpawnTime)
        {
            NewSpawnTime();
        }
    }

    private void NewSpawnTime()
    {
        currentScoreToNextSpawnTime -= scoreToNextSpawnTime; //Reset score
        objectSpawner.spawnTime -= objectSpawner.substractSpawnTime; //Substract time to the spawner so asteroids spawn faster
        if (objectSpawner.spawnTime < 0.1f) //Time can't be 0, minimum should be 0.1f
        {
            objectSpawner.spawnTime = 0.1f;
        }
        objectSpawner.CancelInvoke(); //Cancel the invoke repeating to spawn asteroids
        objectSpawner.SpawnObjects(); //Start the invoke repeating to spawn asteroids with the new spawn time.
    }

    private void CheckAmountLevel() //Used to change spawn time based on current score
    {
        if (currentScoreToNextAmount >= scoreToNextAmount)
        {
            NewAmount();
        }
    }

    private void NewAmount()
    {
        currentScoreToNextAmount -= scoreToNextAmount; //Reset score
        objectSpawner.amountOfObjects++; //Add amount
    }

    public void SetScoreGameOver()
    {
        GameManager.instance.SetHighScore(score);
    }
}
