using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraOffsetController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;  // Reference to the Cinemachine Virtual Camera
    public Transform player;  // Reference to the player's transform

    [Header("Offset Settings")]
    public float minOffsetX = -2f;  // Minimum X value for the offset
    public float maxOffsetX = 2f;   // Maximum X value for the offset
    public float minXPosition = -70f; // The X position where the offset should be at maxOffsetX
    public float maxXPosition = 70f;  // The X position where the offset should be at minOffsetX
    public float lerpSpeed = 2f;    // Speed of the lerp

    private CinemachineComposer composer;  // Reference to the Composer component

    void Start()
    {
        if (virtualCamera != null)
        {
            // Get the Composer component from the virtual camera's Aim section
            composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
        }
    }

    void Update()
    {
        if (player != null && composer != null)
        {
            // Calculate the normalized position of the player within the specified range
            float normalizedX = Mathf.InverseLerp(minXPosition, maxXPosition, player.position.x);

            // Map the normalized value to the offset range
            float targetOffsetX = Mathf.Lerp(maxOffsetX, minOffsetX, normalizedX);

            // Smoothly interpolate the current offset to the target offset
            float newOffsetX = Mathf.Lerp(composer.m_TrackedObjectOffset.x, targetOffsetX, Time.deltaTime * lerpSpeed);

            // Apply the new offset to the Composer's Tracked Object Offset
            composer.m_TrackedObjectOffset = new Vector3(newOffsetX, composer.m_TrackedObjectOffset.y, composer.m_TrackedObjectOffset.z);
        }
    }
}
