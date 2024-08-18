using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealMovement : MonoBehaviour
{
    [Header("Surface Movement Settings")]
    public float steerSpeed = 5f;      // Speed at which the seal steers left and right on the surface
    public float returnSpeed = 2f;     // Speed at which the seal returns to the center (X = 0) on the surface

    [Header("Underwater Movement Settings")]
    public float underwaterSteerSpeed = 3f;  // Speed at which the seal steers left, right, up, and down underwater
    public float underwaterReturnSpeed = 1f; // Speed at which the seal returns to the center (X = 0, Y = -10, Z = 0) underwater
    public Vector3 underwaterCenterOfGravity = new Vector3(0f, -10f, 0f); // Center of gravity underwater

    [Header("Speed Limit Settings")]
    public float maxSpeed = 5f;  // Maximum overall speed limit in any direction

    [Header("Position Constraints")]
    public float minX = -5f;           // Minimum X position
    public float maxX = 5f;            // Maximum X position

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

        if (!isUnderwater)
        {
            // Jump logic when on the surface
            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        else
        {
            // Set the center of gravity underwater to keep the player centered
            rb.centerOfMass = underwaterCenterOfGravity;
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

        // Clamp the player's X position within the min and max range
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        transform.position = clampedPosition;

        // Apply the speed limit
        LimitSpeed();
    }

    void HandleSurfaceMovement()
    {
        // Handle steering (X-axis movement) on the surface
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector3.left * steerSpeed, ForceMode.Acceleration);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector3.right * steerSpeed, ForceMode.Acceleration);
        }
        else if (isGrounded)
        {
            // Gradually return to X = 0 only when grounded
            Vector3 currentPosition = transform.position;
            float returnForceX = (0 - currentPosition.x) * returnSpeed;
            rb.AddForce(Vector3.right * returnForceX, ForceMode.Acceleration);
        }

        // Gradually return to Z = 0
        Vector3 zPosition = transform.position;
        float returnForceZ = (0 - zPosition.z) * returnSpeed;
        rb.AddForce(Vector3.forward * returnForceZ, ForceMode.Acceleration);
    }

    void HandleUnderwaterMovement()
    {
        // Handle steering (X-axis movement) underwater
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector3.left * underwaterSteerSpeed, ForceMode.Acceleration);
        }
        else if (Input.GetKey(KeyCode.D))
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
        Vector3 currentPosition = transform.position;
        float returnForceX = (0 - currentPosition.x) * underwaterReturnSpeed;
        float returnForceY = (underwaterCenterOfGravity.y - currentPosition.y) * underwaterReturnSpeed;
        float returnForceZ = (0 - currentPosition.z) * underwaterReturnSpeed;

        rb.AddForce(new Vector3(returnForceX, returnForceY, returnForceZ), ForceMode.Acceleration);
    }

    void LimitSpeed()
    {
        // Check the current speed of the Rigidbody
        Vector3 velocity = rb.velocity;

        // If the speed exceeds the max speed, clamp it
        if (velocity.magnitude > maxSpeed)
        {
            rb.velocity = velocity.normalized * maxSpeed;
        }
    }
}
