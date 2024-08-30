using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform collectionPoint;       // Collection point (mouth position)
    public TextMeshProUGUI scoreText;       // UI text for the score
    public TextMeshProUGUI gameOverText;    // UI text for Game Over

    public GameObject[] chargeBarSlots;     // UI slots for the charge bar
    private int chargeCount = 0;            // Number of filled slots in the charge bar
    private int score = 0;                  // Player's score

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

        // Initially hide the Game Over text
        gameOverText.gameObject.SetActive(false);
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

        // Allow game restart with "R" key when game is over
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
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

        // Optionally, disable player movement
        sealMovement.enabled = false;

        // Stop all moving segments
        SegmentMovement[] segments = FindObjectsOfType<SegmentMovement>();
        foreach (var segment in segments)
        {
            segment.StopMovement();
        }
    }

    private void RestartGame()
    {
        // Restart the game (reload the scene, reset variables, etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
