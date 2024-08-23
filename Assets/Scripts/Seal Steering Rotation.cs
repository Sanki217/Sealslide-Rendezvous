using UnityEngine;

public class SealSteeringRotation : MonoBehaviour
{
    public float maxZRotation = 30f;  // Maximum Z rotation angle (in degrees)
    public float zRotationMultiplier = 2f;  // Multiplier to amplify the Z rotation
    public float maxXRotation = 20f;  // Maximum X rotation angle (in degrees)
    public float xRotationMultiplier = 1f;  // Multiplier to amplify the X rotation
    public float smoothTime = 0.1f;   // Smooth transition time for rotation

    private Rigidbody playerRigidbody; // Reference to the parent's Rigidbody
    private float currentZRotation;   // Current Z rotation angle
    private float zRotationVelocity;  // Used for smooth transition
    private float currentXRotation;   // Current X rotation angle
    private float xRotationVelocity;  // Used for smooth transition

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
            // --- Z Rotation (Based on Horizontal Speed) ---
            float horizontalSpeed = playerRigidbody.velocity.x;

            // Map the horizontal speed to Z rotation angle and apply the multiplier
            float targetZRotation = -Mathf.Lerp(0f, maxZRotation, Mathf.Abs(horizontalSpeed) / maxZRotation) * zRotationMultiplier;

            // Adjust the target rotation direction based on left or right movement
            if (horizontalSpeed < 0)
            {
                targetZRotation = -targetZRotation;
            }

            // Smoothly rotate the seal to the target Z rotation
            currentZRotation = Mathf.SmoothDamp(currentZRotation, targetZRotation, ref zRotationVelocity, smoothTime);

            // --- X Rotation (Based on Vertical Speed) ---
            float verticalSpeed = playerRigidbody.velocity.y;

            // Map the vertical speed to X rotation angle and apply the multiplier
            float targetXRotation = -Mathf.Lerp(0f, maxXRotation, Mathf.Abs(verticalSpeed) / maxXRotation) * xRotationMultiplier;

            // Adjust the target rotation direction based on upward or downward movement
            if (verticalSpeed < 0)
            {
                targetXRotation = -targetXRotation;
            }

            // Smoothly rotate the seal to the target X rotation
            currentXRotation = Mathf.SmoothDamp(currentXRotation, targetXRotation, ref xRotationVelocity, smoothTime);

            // Apply the rotation to the seal (rotate on both X and Z axes)
            transform.localRotation = Quaternion.Euler(currentXRotation, transform.localRotation.eulerAngles.y, currentZRotation);
        }
    }
}
