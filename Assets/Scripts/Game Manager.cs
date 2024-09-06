using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform collectionPoint;       // Collection point (mouth position)
    public TextMeshProUGUI scoreText;       // UI text for the score
    public TextMeshProUGUI gameOverText;    // UI text for Game Over
    public TextMeshProUGUI pressAnyKeyText; // UI text for "Press any key"

    public GameObject[] chargeBarSlots;     // UI slots for the charge bar
    private int chargeCount = 0;            // Number of filled slots in the charge bar
    private int score = 0;                  // Player's score (fish collected in the current game)

    public float dashForce = 50f;           // Force applied during dash
    private Rigidbody playerRigidbody;      // Reference to the player's Rigidbody

    private SealMovement sealMovement;      // Reference to the player's movement script

    private bool isGameOver = false;        // Flag to track game over state

    private void Awake()
    {
        // Singleton pattern to ensure only one GameManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Find the player's Rigidbody and movement script
        sealMovement = FindObjectOfType<SealMovement>();
        playerRigidbody = sealMovement.GetComponent<Rigidbody>();

        // Initially hide the Game Over and "Press any key" texts
        gameOverText.gameObject.SetActive(false);
        pressAnyKeyText.gameObject.SetActive(false);
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        scoreText.text = "Fish: " + score.ToString();
    }

    public void AddFishToChargeBar()
    {
        if (chargeCount < chargeBarSlots.Length)
        {
            chargeBarSlots[chargeCount].SetActive(true);
            chargeCount++;
        }
    }

    private void ResetChargeBar()
    {
        for (int i = 0; i < chargeBarSlots.Length; i++)
        {
            chargeBarSlots[i].SetActive(false);
        }
        chargeCount = 0;
    }

    void Update()
    {
        // Check if the player presses Shift and has 5 fish in the charge bar
        if (!isGameOver && Input.GetKeyDown(KeyCode.LeftShift) && chargeCount == chargeBarSlots.Length)
        {
            PerformDash();
        }

        // Check for any key press after game over to go to the main menu
        if (isGameOver && Input.anyKeyDown)
        {
            GoToMainMenu();
        }
    }

    private void PerformDash()
    {
        // Apply dash force in the current direction of the player
        Vector3 dashDirection = playerRigidbody.velocity.normalized;
        playerRigidbody.AddForce(dashDirection * dashForce, ForceMode.VelocityChange);

        // Reset the charge bar
        ResetChargeBar();
    }

    public void TriggerGameOver()
    {
        isGameOver = true;
        gameOverText.gameObject.SetActive(true);  // Show Game Over text

        // Stop the score from increasing
        FindObjectOfType<PointsManager>().StopScoring();

        // Save collected fish to total fish (persistent storage)
        int totalFish = PlayerPrefs.GetInt("TotalFish", 0);
        totalFish += score;  // Add the fish collected in this session to the total
        PlayerPrefs.SetInt("TotalFish", totalFish);

        // Show "Press any key to continue" text
        pressAnyKeyText.gameObject.SetActive(true);

        // Optionally, disable player movement
        sealMovement.enabled = false;

        // Stop all moving segments
        SegmentMovement[] segments = FindObjectsOfType<SegmentMovement>();
        foreach (var segment in segments)
        {
            segment.StopMovement();
        }
    }

    private void GoToMainMenu()
    {
        // Load the main menu scene (replace "MainMenu" with your main menu scene's name)
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
