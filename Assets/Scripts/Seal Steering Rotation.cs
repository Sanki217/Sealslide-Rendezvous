using UnityEngine;

public class SealSteeringRotation : MonoBehaviour
{
    public float maxZRotation = 30f;  // Maximum Z rotation angle (in degrees)
    public float zRotationMultiplier = 2f;  // Multiplier to amplify the Z rotation
    public float maxXRotation = 20f;  // Maximum X rotation angle (in degrees)
    public float xRotationMultiplier = 1f;  // Multiplier to amplify the X rotation
    public float smoothTime = 0.2f;   // Smooth transition time for rotation
    public float yReturnSpeed = 2f;   // Speed at which Y rotation returns to 0 when no input

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
            float targetZRotation = -normalizedHorizontalSpeed * maxZRotation * zRotationMultiplier;

            // --- X Rotation (Based on Normalized Vertical Speed) ---
            float normalizedVerticalSpeed = Mathf.Clamp(playerRigidbody.velocity.y / 10f, -1f, 1f);
            float targetXRotation = -normalizedVerticalSpeed * maxXRotation * xRotationMultiplier;

            // --- Y Rotation Return to 0 (When No Input) ---
            float targetYRotation = transform.localRotation.eulerAngles.y;
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                // Smoothly return Y rotation to 0 when there is no input
                targetYRotation = Mathf.LerpAngle(transform.localRotation.eulerAngles.y, 0f, Time.fixedDeltaTime * yReturnSpeed);
            }

            // Set the target rotation with X, Y, and Z rotations
            targetRotation = Quaternion.Euler(targetXRotation, targetYRotation, targetZRotation);

            // Smoothly interpolate from current rotation to target rotation
            currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime / smoothTime);

            // Apply the rotation to the seal
            transform.localRotation = currentRotation;
        }
    }
}
