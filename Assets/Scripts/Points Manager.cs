using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsManager : MonoBehaviour
{
    public int score = 0;
    public float scoreMultiplier = 1f;
    public float pointsPerSecond = 10f;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;  // Text element to display the high score

    private float timeSinceLastPoints = 0f;
    private bool isGameOver = false;

    private void Start()
    {
        // Load the high score from PlayerPrefs
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore.ToString();
    }

    private void Update()
    {
        if (!isGameOver)
        {
            timeSinceLastPoints += Time.deltaTime;

            if (timeSinceLastPoints >= 1f / pointsPerSecond)
            {
                timeSinceLastPoints = 0f;
                int pointsToAdd = (int)(pointsPerSecond * scoreMultiplier);
                score += pointsToAdd;
                scoreText.text = "Score: " + score.ToString();
            }
        }
    }

    public void StopScoring()
    {
        isGameOver = true;

        // Check if the current score is higher than the high score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            // Save the new high score
            PlayerPrefs.SetInt("HighScore", score);
            highScoreText.text = "High Score: " + score.ToString();
        }
    }
}
