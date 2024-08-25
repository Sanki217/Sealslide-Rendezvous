using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform collectionPoint;       // Collection point (mouth position)
    public TextMeshProUGUI scoreText;       // UI text for the score

    public GameObject[] chargeBarSlots;     // UI slots for the charge bar
    private int chargeCount = 0;            // Number of filled slots in the charge bar
    private int score = 0;                  // Player's score

    public float dashForce = 50f;           // Force applied during dash
    private Rigidbody playerRigidbody;      // Reference to the player's Rigidbody

    private SealMovement sealMovement;      // Reference to the player's movement script

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
            // Update the UI charge bar
            FindObjectOfType<ChargeBarUIManager>().UpdateChargeBar(chargeCount);
        }
    }

    private void ResetChargeBar()
    {
        for (int i = 0; i < chargeBarSlots.Length; i++)
        {
            chargeBarSlots[i].SetActive(false);
        }
        chargeCount = 0;
        // Update the UI charge bar
        FindObjectOfType<ChargeBarUIManager>().UpdateChargeBar(chargeCount);
    }


    void Update()
    {
        // Check if the player presses Shift and has 5 fish in the charge bar
        if (Input.GetKeyDown(KeyCode.LeftShift) && chargeCount == chargeBarSlots.Length)
        {
            PerformDash();
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

}
