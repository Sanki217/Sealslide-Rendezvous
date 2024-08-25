using UnityEngine;

public class SealSteeringRotation : MonoBehaviour
{
    public float maxZRotation = 30f;  // Maximum Z rotation angle (in degrees)
    public float zRotationMultiplier = 2f;  // Multiplier to amplify the Z rotation
    public float maxXRotation = 20f;  // Maximum X rotation angle (in degrees)
    public float xRotationMultiplier = 1f;  // Multiplier to amplify the X rotation
    public float smoothTime = 0.2f;   // Smooth transition time for rotation

    private Rigidbody playerRigidbody; // Reference to the parent's Rigidbody
    private Quaternion targetRotation; // Target rotation based on player movement
    private Quaternion currentRotation; // Current rotation of the seal

    void Start()
    {
        // Get the Rigidbody component from the parent (Player)
        playerRigidbody = GetComponentInParent<Rigidbody>();
        currentRotation = transform.localRotation; // Initialize current rotation
    }

    void FixedUpdate()
    {
        // Ensure we have the player's Rigidbody
        if (playerRigidbody != null)
        {
            // --- Z Rotation (Based on Normalized Horizontal Speed) ---
            float normalizedHorizontalSpeed = Mathf.Clamp(playerRigidbody.velocity.x / 10f, -1f, 1f);

            // Calculate target Z rotation with normalized speed
            float targetZRotation = -normalizedHorizontalSpeed * maxZRotation * zRotationMultiplier;

            // --- X Rotation (Based on Normalized Vertical Speed) ---
            float normalizedVerticalSpeed = Mathf.Clamp(playerRigidbody.velocity.y / 10f, -1f, 1f);

            // Calculate target X rotation with normalized speed
            float targetXRotation = -normalizedVerticalSpeed * maxXRotation * xRotationMultiplier;

            // Set the target rotation with both X and Z rotations
            targetRotation = Quaternion.Euler(targetXRotation, transform.localRotation.eulerAngles.y, targetZRotation);

            // Smoothly interpolate from current rotation to target rotation
            currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime / smoothTime);

            // Apply the rotation to the seal
            transform.localRotation = currentRotation;
        }
    }
}