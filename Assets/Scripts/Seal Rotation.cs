using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustSealRotation : MonoBehaviour
{
    public Transform seal;  // Reference to the Seal object
    public float raycastDistance = 5f;  // How far the raycast checks for the ground
    public AnimationCurve rotationSpeedCurve;  // Animation curve to control rotation speed

    private float timeSinceLastAdjustment = 0f;  // Timer to track how long the adjustment has been happening

    private void Update()
    {
        AdjustRotationBasedOnSlope();
    }

    private void AdjustRotationBasedOnSlope()
    {
        RaycastHit hit;

        // Cast a ray downward from the player's position to detect the ground
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            // Get the normal of the hit surface
            Vector3 normal = hit.normal;

            // Calculate the desired Z rotation based on the normal vector and reverse it
            float targetZRotation = -Mathf.Atan2(normal.x, normal.y) * Mathf.Rad2Deg;

            // Get the rotation speed from the curve
            float rotationSpeed = rotationSpeedCurve.Evaluate(timeSinceLastAdjustment);

            // Smoothly rotate the Seal object towards the desired Z rotation
            seal.localRotation = Quaternion.Lerp(seal.localRotation, Quaternion.Euler(0, 0, targetZRotation), rotationSpeed * Time.deltaTime);

            // Increase the timer
            timeSinceLastAdjustment += Time.deltaTime;
        }
        else
        {
            // No object below, smoothly rotate back to Z rotation 0
            float rotationSpeed = rotationSpeedCurve.Evaluate(timeSinceLastAdjustment);
            seal.localRotation = Quaternion.Lerp(seal.localRotation, Quaternion.Euler(0, 0, 0), rotationSpeed * Time.deltaTime);

            // Increase the timer
            timeSinceLastAdjustment += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Reset the timer when entering a new slope or air
        timeSinceLastAdjustment = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the raycast in the scene view for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastDistance);
    }
}
