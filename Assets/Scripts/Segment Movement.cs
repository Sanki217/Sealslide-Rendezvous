using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentMovement : MonoBehaviour
{
    public float initialSpeed = 10f;   // Initial speed of the first segment
    public float acceleration = 2f;    // Acceleration rate (units per second)
    public float currentSpeed;        // The current speed, which will increase over time
    private Vector3 targetPosition;    // The target position for the segment

    public float destroyZPosition = -10f;  // The Z position at which the segment should be destroyed

    // Public property to access currentSpeed
    public float CurrentSpeed
    {
        get { return currentSpeed; }
    }

    void Start()
    {
        currentSpeed = initialSpeed;   // Set the initial speed for the first platform
        targetPosition = transform.position;  // Start at the current position
    }

    void FixedUpdate()
    {
        // Calculate the new speed with acceleration
        currentSpeed += acceleration * Time.fixedDeltaTime;

        // Calculate the target position based on the speed
        targetPosition += Vector3.back * currentSpeed * Time.fixedDeltaTime;

        // Smoothly move towards the target position using Lerp
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);  // Adjust the Lerp factor for desired smoothness

        // Check if the segment is past the destroy position
        if (transform.position.z <= destroyZPosition)
        {
            Destroy(gameObject);
        }
    }

    // Method to set the initial speed (used by the spawner)
    public void SetInitialSpeed(float speed)
    {
        currentSpeed = speed;
    }
}
