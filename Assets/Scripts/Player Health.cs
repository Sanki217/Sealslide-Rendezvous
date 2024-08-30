using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 2;  // Maximum health
    public int currentHealth;  // Current health
    public float regenTime = 15f;  // Time it takes to regenerate health

    private float regenTimer;  // Timer for health regeneration
    public TextMeshProUGUI healthText;  // Reference to the UI Text for displaying health
    public GameObject gameOverText;  // Reference to the Game Over UI Text

    public GameObject dangerIndicator;  // The red cube or danger indicator

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        dangerIndicator.SetActive(false);  // Hide the danger indicator initially

        if (gameOverText != null)
        {
            gameOverText.SetActive(false);  // Hide game over text at the start
        }
    }

    void Update()
    {
        // Regenerate health if not at max health and player is not at 0 health
        if (currentHealth < maxHealth && currentHealth > 0)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= regenTime)
            {
                currentHealth++;
                regenTimer = 0f;
                UpdateHealthUI();
            }
        }

        // Show danger indicator if health is 1
        if (currentHealth == 1)
        {
            dangerIndicator.SetActive(true);
        }
        else
        {
            dangerIndicator.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        regenTimer = 0f;  // Reset the regeneration timer when taking damage
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            UpdateHealthUI();
            GameOver();
        }
    }

    void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth;
    }

    void GameOver()
    {
        // Display the Game Over text
        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }
        else
        {
            Debug.LogError("GameOverText is not assigned in the Inspector!");
        }

        // Stop the game
        GameManager.Instance.TriggerGameOver();
    }
}
