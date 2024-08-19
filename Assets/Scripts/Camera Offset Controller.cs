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
            // Calculate the target offset based on the player's X position
            float targetOffsetX = Mathf.Lerp(minOffsetX, maxOffsetX, (player.position.x + 1) / 2);

            // Clamp the target offset between the minimum and maximum values
            targetOffsetX = Mathf.Clamp(targetOffsetX, minOffsetX, maxOffsetX);

            // Smoothly interpolate the current offset to the target offset
            float newOffsetX = Mathf.Lerp(composer.m_TrackedObjectOffset.x, targetOffsetX, Time.deltaTime * lerpSpeed);

            // Apply the new offset to the Composer's Tracked Object Offset
            composer.m_TrackedObjectOffset = new Vector3(newOffsetX, composer.m_TrackedObjectOffset.y, composer.m_TrackedObjectOffset.z);
        }
    }
}
