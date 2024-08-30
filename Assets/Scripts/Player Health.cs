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

    public GameObject dangerIndicator;  // The red cube or danger indicator

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        dangerIndicator.SetActive(false);  // Hide the danger indicator initially
    }

    void Update()
    {
        // Regenerate health if not at max health
        if (currentHealth < maxHealth)
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
            // Handle game over
            GameOver();
        }
    }

    void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth;
    }

    void GameOver()
    {
        // Implement game over logic here (e.g., stop the game, show game over screen)
        Debug.Log("Game Over");
    }
}
