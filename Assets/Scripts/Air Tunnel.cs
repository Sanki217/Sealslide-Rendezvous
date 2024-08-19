using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTunnel : MonoBehaviour
{
    public Transform[] tunnelPath;   // Array of waypoints that define the tunnel's path
    public float followSpeed = 5f;   // Speed at which the player moves along the path
    public float closeEnoughDistance = 0.1f;  // Distance to consider the player has reached a waypoint

    private Rigidbody playerRb;
    private Transform playerTransform;
    private bool isInTunnel = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRb = other.GetComponent<Rigidbody>();
            playerTransform = other.transform;

            // Disable gravity and stop player movement when entering the tunnel
            playerRb.useGravity = false;
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;

            isInTunnel = true;

            // Start following the tunnel path
            StartCoroutine(FollowTunnelPath());
        }
    }

    private IEnumerator FollowTunnelPath()
    {
        foreach (Transform waypoint in tunnelPath)
        {
            // Move towards each waypoint in sequence
            while (Vector3.Distance(playerTransform.position, waypoint.position) > closeEnoughDistance)
            {
                if (!isInTunnel) yield break;  // Exit if the player leaves the tunnel

                // Calculate the next position towards the waypoint
                Vector3 nextPosition = Vector3.MoveTowards(playerTransform.position, waypoint.position, followSpeed * Time.deltaTime);

                // Keep the player's Z position the same
                nextPosition.z = playerTransform.position.z;

                // Update the player's position
                playerTransform.position = nextPosition;

                yield return null;
            }
        }

        // Stop the player's movement after reaching the last waypoint
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTunnel = false;

            // Re-enable gravity when exiting the tunnel
            playerRb.useGravity = true;

            // Ensure the player stops any leftover movement
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }
    }
}
