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

    private float timeSinceLastPoints = 0f;

    private void Update()
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