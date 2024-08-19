using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiGravityFloor : MonoBehaviour
{
    public float upwardForce = 50f; // Strength of the upward force

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                // Apply only upward force
                playerRb.AddForce(Vector3.up * upwardForce, ForceMode.Acceleration);
            }
        }
    }
}