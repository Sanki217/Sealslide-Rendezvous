using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;  // Text to display the high score
    public TextMeshProUGUI totalFishText;  // Text to display total fish collected

    private void Start()
    {
        // Load and display the high score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore.ToString();

        // Load and display the total fish collected
        int totalFish = PlayerPrefs.GetInt("TotalFish", 0);
        totalFishText.text = "Fish Collected: " + totalFish.ToString();
    }

    public void StartGame()
    {
        // Load the game scene (replace "GameScene" with the actual scene name)
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}