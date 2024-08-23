using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform collectionPoint;    // Player's mouth transform
    public TextMeshProUGUI scoreText;    // Reference to the score text in the UI

    private int score = 0;               // Player's score

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        scoreText.text = "Fish: " + score.ToString();
    }
}
