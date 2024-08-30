using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float bounceForce = 10f; // The force with which the trampoline will bounce the player
    public Transform normalIndicator; // A visible arrow showing the bounce direction in the scene view

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            // Calculate the bounce direction using the trampoline's normal
            Vector3 bounceDirection = transform.up; // Change this if you want the trampoline to bounce in different directions
            playerRigidbody.velocity = Vector3.zero; // Reset current velocity
            playerRigidbody.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
        }
    }

    void OnDrawGizmos()
    {
        // Draw the bounce direction in the scene view
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 2f);
    }
}
