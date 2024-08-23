using UnityEngine;

public class SealSteeringRotation : MonoBehaviour
{
    public float maxZRotation = 30f;  // Maximum Z rotation angle (in degrees)
    public float rotationMultiplier = 2f;  // Multiplier to amplify the rotation
    public float smoothTime = 0.1f;   // Smooth transition time for rotation

    private Rigidbody playerRigidbody; // Reference to the parent's Rigidbody
    private float currentZRotation;   // Current Z rotation angle
    private float zRotationVelocity;  // Used for smooth transition

    void Start()
    {
        // Get the Rigidbody component from the parent (Player)
        playerRigidbody = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        // Ensure we have the player's Rigidbody
        if (playerRigidbody != null)
        {
            // Get horizontal velocity (X axis) from the parent Rigidbody
            float horizontalSpeed = playerRigidbody.velocity.x;

            // Map the horizontal speed to Z rotation angle and apply the multiplier
            float targetZRotation = -Mathf.Lerp(0f, maxZRotation, Mathf.Abs(horizontalSpeed) / maxZRotation) * rotationMultiplier;

            // Adjust the target rotation direction based on left or right movement
            if (horizontalSpeed < 0)
            {
                targetZRotation = -targetZRotation;
            }

            // Smoothly rotate the seal to the target Z rotation
            currentZRotation = Mathf.SmoothDamp(currentZRotation, targetZRotation, ref zRotationVelocity, smoothTime);

            // Apply the rotation to the seal (only rotate on Z axis)
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, currentZRotation);
        }
    }
}
