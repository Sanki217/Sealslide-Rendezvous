using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int damageAmount = 1;  // Amount of damage dealt to the player

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object collided with is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerHealth component from the player
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Inflict damage on the player
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}
