using UnityEngine;

public class SegmentMovement : MonoBehaviour
{
    public float initialSpeed = 10f;    // Initial speed of the first segment
    public float acceleration = 2f;     // Acceleration rate (units per second)
    public float maxSpeed = 50f;        // Maximum speed the segment can reach
    private float currentSpeed;         // The current speed, which will increase over time
    private Vector3 targetPosition;     // The target position for the segment

    public float destroyZPosition = -10f;  // The Z position at which the segment should be destroyed

    private bool isMoving = true;  // Flag to track if the segment should keep moving

    public float CurrentSpeed
    {
        get { return currentSpeed; }
    }

    void Start()
    {
        if (currentSpeed == 0)
        {
            currentSpeed = initialSpeed;
        }

        targetPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (!isMoving) return;  // Stop moving if the game is over

        // Calculate the new speed with acceleration
        currentSpeed += acceleration * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

        // Calculate the target position based on the speed
        targetPosition += Vector3.back * currentSpeed * Time.fixedDeltaTime;

        // Smoothly move towards the target position using Lerp
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);

        // Check if the segment is past the destroy position
        if (transform.position.z <= destroyZPosition)
        {
            Destroy(gameObject);
        }
    }

    public void StopMovement()
    {
        isMoving = false;
    }

    // Method to set the initial speed (used by the spawner)
    public void SetInitialSpeed(float speed)
    {
        currentSpeed = speed;
    }
}
