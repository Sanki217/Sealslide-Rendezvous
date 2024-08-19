using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SealMovement : MonoBehaviour
{
    [Header("Surface Movement Settings")]
    public float steerSpeed = 5f;
    public float returnSpeed = 2f;

    [Header("Underwater Movement Settings")]
    public float underwaterSteerSpeed = 3f;
    public float underwaterReturnSpeed = 1f;
    public Vector3 underwaterCenterOfGravity = new Vector3(0f, -10f, 0f);

    [Header("Speed Limit Settings")]
    public float maxXSpeed = 5f;
    public float maxYSpeed = 5f;

    [Header("Position Constraints")]
    public float minX = -50f;
    public float maxX = 50f;

    [Header("Camera Settings")]
    public CinemachineVirtualCamera virtualCamera; 
    public float minZOffset = -10f; 
    public float maxZOffset = -30f; 
    public float maxSpeedForZOffset = 10f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.5f;
    [SerializeField] private float groundCheckRadius = 0.3f;

    [Header("Gravity Settings")]
    [SerializeField] private float gravityModifier = 1f;

    private Rigidbody rb;
    private Vector3 startPosition;
    private bool isGrounded;
    private bool isUnderwater;

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

        // Adjust the camera Z offset based on the player's speed
        AdjustCameraZOffset();
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

        if (Input.GetKey(KeyCode.A) && currentPosition.x > minX)
        {
            rb.AddForce(Vector3.left * underwaterSteerSpeed, ForceMode.Acceleration);
        }
        else if (Input.GetKey(KeyCode.D) && currentPosition.x < maxX)
        {
            rb.AddForce(Vector3.right * underwaterSteerSpeed, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.up * underwaterSteerSpeed, ForceMode.Acceleration);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector3.down * underwaterSteerSpeed, ForceMode.Acceleration);
        }

        // Gradually return to center of gravity underwater
        float returnForceX = (0 - currentPosition.x) * underwaterReturnSpeed;
        float returnForceY = (underwaterCenterOfGravity.y - currentPosition.y) * underwaterReturnSpeed;
        float returnForceZ = (0 - currentPosition.z) * underwaterReturnSpeed;

        rb.AddForce(new Vector3(returnForceX, returnForceY, returnForceZ), ForceMode.Acceleration);
    }

    void LimitSpeed()
    {
        Vector3 velocity = rb.velocity;
        if (Mathf.Abs(velocity.x) > maxXSpeed)
        {
            velocity.x = Mathf.Sign(velocity.x) * maxXSpeed;
        }

        if (Mathf.Abs(velocity.y) > maxYSpeed)
        {
            velocity.y = Mathf.Sign(velocity.y) * maxYSpeed;
        }

        rb.velocity = velocity;
    }

    void AdjustCameraZOffset()
    {
        float currentSpeed = rb.velocity.magnitude;

        float t = Mathf.Clamp01(currentSpeed / maxSpeedForZOffset);
        float targetZOffset = Mathf.Lerp(minZOffset, maxZOffset, t);

        Vector3 followOffset = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        followOffset.z = Mathf.Lerp(followOffset.z, targetZOffset, Time.deltaTime * returnSpeed);
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = followOffset;
    }

}
