using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealMovement : MonoBehaviour
{
    [Header("Surface Movement Settings")]
    public float steerSpeed = 5f;      // Speed at which the seal steers left and right on the surface
    public float returnSpeed = 2f;     // Speed at which the seal returns to the center (X = 0) on the surface
    //public Vector3 surfaceCenterOfGravity = new Vector3(0f, -1f, 0f); // Center of gravity on the surface

    [Header("Underwater Movement Settings")]
    public float underwaterSteerSpeed = 3f;  // Speed at which the seal steers left, right, up, and down underwater
    public float underwaterReturnSpeed = 1f; // Speed at which the seal returns to the center (X = 0, Y = -10, Z = 0) underwater
    public Vector3 underwaterCenterOfGravity = new Vector3(0f, -10f, 0f); // Center of gravity underwater

    [Header("Speed Limit Settings")]
    public float maxXSpeed = 5f;  // Maximum speed in the X direction
    public float maxYSpeed = 5f;  // Maximum speed in the Y direction

    [Header("Position Constraints")]
    public float minX = -50f;  // Minimum X position to stop steering left
    public float maxX = 50f;   // Maximum X position to stop steering right

    private Rigidbody rb;
    private Vector3 startPosition;
    private bool isGrounded;
    private bool isUnderwater;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f; // Jump force
    [SerializeField] private LayerMask groundLayer; // Layer for ground detection
    [SerializeField] private float groundCheckDistance = 0.5f; // Distance for ground check
    [SerializeField] private float groundCheckRadius = 0.3f; // Radius for ground check sphere

    [Header("Gravity Settings")]
    [SerializeField] private float gravityModifier = 1f; // Modifier for gravity

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;

        Physics.gravity *= gravityModifier;
    }

    private void Update()
    {
        isGrounded = Physics.SphereCast(transform.position, groundCheckRadius, Vector3.down, out RaycastHit hitInfo, groundCheckDistance, groundLayer);
        isUnderwater = transform.position.y < 0;

        if (isUnderwater)
        {
            // Set the center of gravity underwater to keep the player centered
            rb.centerOfMass = underwaterCenterOfGravity;
        }
        else
        {
            // Set the center of gravity for surface movement
            // rb.centerOfMass = surfaceCenterOfGravity;

            // Jump logic when on the surface
            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    void FixedUpdate()
    {
        if (isUnderwater)
        {
            HandleUnderwaterMovement();
        }
        else
        {
            HandleSurfaceMovement();
        }

        // Apply the speed limits for X and Y axes
        LimitSpeed();
    }

    void HandleSurfaceMovement()
    {
        Vector3 currentPosition = transform.position;

        // Handle steering (X-axis movement) on the surface
        if (Input.GetKey(KeyCode.A) && currentPosition.x > minX)
        {
            rb.AddForce(Vector3.left * steerSpeed, ForceMode.Acceleration);
        }
        else if (Input.GetKey(KeyCode.D) && currentPosition.x < maxX)
        {
            rb.AddForce(Vector3.right * steerSpeed, ForceMode.Acceleration);
        }
        else if (isGrounded)
        {
            // Gradually return to X = 0 only when grounded
            float returnForceX = (0 - currentPosition.x) * returnSpeed;
            rb.AddForce(Vector3.right * returnForceX, ForceMode.Acceleration);
        }

        // Gradually return to Z = 0
        float returnForceZ = (0 - currentPosition.z) * returnSpeed;
        rb.AddForce(Vector3.forward * returnForceZ, ForceMode.Acceleration);
    }

    void HandleUnderwaterMovement()
    {
        Vector3 currentPosition = transform.position;

        // Handle steering (X-axis movement) underwater
        if (Input.GetKey(KeyCode.A) && currentPosition.x > minX)
        {
            rb.AddForce(Vector3.left * underwaterSteerSpeed, ForceMode.Acceleration);
        }
        else if (Input.GetKey(KeyCode.D) && currentPosition.x < maxX)
        {
            rb.AddForce(Vector3.right * underwaterSteerSpeed, ForceMode.Acceleration);
        }

        // Handle vertical movement (Y-axis) underwater
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.up * underwaterSteerSpeed, ForceMode.Acceleration);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector3.down * underwaterSteerSpeed, ForceMode.Acceleration);
        }

        // Gradually return to X = 0, Y = -10, and Z = 0 underwater
        float returnForceX = (0 - currentPosition.x) * underwaterReturnSpeed;
        float returnForceY = (underwaterCenterOfGravity.y - currentPosition.y) * underwaterReturnSpeed;
        float returnForceZ = (0 - currentPosition.z) * underwaterReturnSpeed;

        rb.AddForce(new Vector3(returnForceX, returnForceY, returnForceZ), ForceMode.Acceleration);
    }

    void LimitSpeed()
    {
        // Limit the speed in the X direction
        Vector3 velocity = rb.velocity;
        if (Mathf.Abs(velocity.x) > maxXSpeed)
        {
            velocity.x = Mathf.Sign(velocity.x) * maxXSpeed;
        }

        // Limit the speed in the Y direction
        if (Mathf.Abs(velocity.y) > maxYSpeed)
        {
            velocity.y = Mathf.Sign(velocity.y) * maxYSpeed;
        }

        // Assign the limited velocity back to the Rigidbody
        rb.velocity = velocity;
    }
}
