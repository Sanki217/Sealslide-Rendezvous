using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    
    [Header("Movement Settings")]
    [SerializeField] private float startingMovementSpeed = 2f; // Starting forward speed
    [SerializeField] private float maxMovementSpeed = 5f; // Maximum forward speed
    [SerializeField] private float acceleration = 2f; // Acceleration rate

    [Header("Steering Settings")]
    [SerializeField] private float steerSpeed = 2f; // How fast the player can steer
    [SerializeField] private float maxSteerAngle = 30f; // Maximum angle for steering

   
    [Header("Rotation Limits")]
    [SerializeField] private float minYRotation = -45f; // Minimum Y rotation
    [SerializeField] private float maxYRotation = 45f; // Maximum Y rotation

 
    [Header("Return to Default Settings")]
    [SerializeField] private float returnSpeed = 1f; // Speed of returning to default rotation

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f; // Jump force
    [SerializeField] private LayerMask groundLayer; // Layer for ground detection
    [SerializeField] private float groundCheckDistance = 0.5f; // Distance for ground check
    [SerializeField] private float groundCheckRadius = 0.3f; // Radius for ground check sphere

    [Header("Gravity Settings")]
    [SerializeField] private float gravityModifier = 1f; // Modifier for gravity

    private Rigidbody rb;
    private float currentSteeringAngle;
    public float currentMovementSpeed; 
    private bool isGrounded;

    // Reference to the seal (child object)
    private Transform sealTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from this GameObject.");
        }

        // Initialize the current movement speed
        currentMovementSpeed = startingMovementSpeed;

        // Apply initial gravity modifier
        Physics.gravity *= gravityModifier;
    }

    private void Update()
    {
        // Check if the player is grounded using a SphereCast
        isGrounded = Physics.SphereCast(transform.position, groundCheckRadius, Vector3.down, out RaycastHit hitInfo, groundCheckDistance, groundLayer);

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
            // Smoothly return to a Y rotation of 0 relative to the world when no input is detected
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

        // Align the seal with the slope
        if (isGrounded)
        {
            Vector3 groundNormal = hitInfo.normal;
            Quaternion targetSealRotation = Quaternion.FromToRotation(Vector3.up, groundNormal);
            sealTransform.rotation = Quaternion.Lerp(sealTransform.rotation, targetSealRotation, returnSpeed * Time.deltaTime);
        }
    }

    // FixedUpdate is called at a consistent interval for physics calculations
    private void FixedUpdate()
    {
        // Move the player forward based on the current movement speed
        Vector3 forwardMovement = transform.forward * currentMovementSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMovement);

        // Ensure the Y rotation is relative to the world, not local space
        Quaternion worldYRotation = Quaternion.Euler(0, currentSteeringAngle, 0);
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, worldYRotation, returnSpeed * Time.fixedDeltaTime));

        // Lock player X and Z rotation to 0
        Vector3 lockedRotation = rb.rotation.eulerAngles;
        lockedRotation.x = 0;
        lockedRotation.z = 0;
        rb.rotation = Quaternion.Euler(lockedRotation);
    }

  
}