
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement speed settings
    [Header("Movement Settings")]
    [SerializeField] private float startingMovementSpeed = 2f; // Starting forward speed
    [SerializeField] private float maxMovementSpeed = 5f; // Maximum forward speed
    [SerializeField] private float acceleration = 2f; // Acceleration rate

    [Header("Steering Settings")]
    [SerializeField] private float steerSpeed = 2f; // How fast the player can steer
    [SerializeField] private float maxSteerAngle = 30f; // Maximum angle for steering

    // Y Rotation limits
    [Header("Rotation Limits")]
    [SerializeField] private float minYRotation = -45f; // Minimum Y rotation
    [SerializeField] private float maxYRotation = 45f; // Maximum Y rotation

    // Return to default rotation settings
    [Header("Return to Default Settings")]
    [SerializeField] private float returnSpeed = 1f; // Speed of returning to default Y rotation

    // Jump settings
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f; // Jump force
    [SerializeField] private LayerMask groundLayer; // Layer for ground detection
    [SerializeField] private float groundCheckDistance = 0.1f; // Distance for ground check

    // Reference to the Rigidbody component
    private Rigidbody rb;

    // Current steering angle
    private float currentSteeringAngle;

    // Current movement speed
    [HideInInspector]
    public float currentMovementSpeed; // Start at startingMovementSpeed

    // Ground check state
    private bool isGrounded;

    // Initialize the Rigidbody component
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from this GameObject.");
        }

        // Initialize the current movement speed
        currentMovementSpeed = startingMovementSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        // Get input for steering
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)
        {
            // Calculate the desired steering angle based on input
            currentSteeringAngle += horizontalInput * steerSpeed * Time.deltaTime;
            currentSteeringAngle = Mathf.Clamp(currentSteeringAngle, minYRotation, maxYRotation);
        }
        else
        {
            // Smoothly return to a Y rotation of 0 when no input is detected
            currentSteeringAngle = Mathf.MoveTowards(currentSteeringAngle, 0, returnSpeed * Time.deltaTime);
        }

        // Accelerate the movement speed
        if (currentMovementSpeed < maxMovementSpeed)
        {
            currentMovementSpeed += acceleration * Time.deltaTime; // Increase speed over time
            currentMovementSpeed = Mathf.Clamp(currentMovementSpeed, startingMovementSpeed, maxMovementSpeed); // Ensure it doesn't exceed max speed
        }

        // Jump logic
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // FixedUpdate is called at a consistent interval for physics calculations
    private void FixedUpdate()
    {
        // Move the player forward based on the current movement speed
        Vector3 forwardMovement = transform.forward * currentMovementSpeed * Time.fixedDeltaTime;

        // Apply forward movement to the Rigidbody
        rb.MovePosition(rb.position + forwardMovement);

        // Clamp Y rotation
        float clampedYRotation = Mathf.Clamp(currentSteeringAngle, minYRotation, maxYRotation);
        Quaternion targetRotation = Quaternion.Euler(0, clampedYRotation, 0);
        
        // Smoothly interpolate rotation towards the target rotation
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, returnSpeed * Time.fixedDeltaTime));
    }
}