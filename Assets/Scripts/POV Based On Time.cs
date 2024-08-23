using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class POVBasedOnTime : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;  // Reference to the virtual camera
    public float minFOV = 60f;  // Minimum Field of View
    public float maxFOV = 85f;  // Maximum Field of View
    public float smoothTime = 0.1f;  // Time for smooth transition

    private float currentVelocity = 0f;  // Used for smooth transition

    void Update()
    {
        // Calculate target FOV based on time scale
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, Time.timeScale);

        // Smoothly change the FOV
        float currentFOV = virtualCamera.m_Lens.FieldOfView;
        virtualCamera.m_Lens.FieldOfView = Mathf.SmoothDamp(currentFOV, targetFOV, ref currentVelocity, smoothTime);
    }
}