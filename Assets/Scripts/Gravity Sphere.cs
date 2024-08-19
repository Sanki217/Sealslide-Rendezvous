using UnityEngine;

public class GravitySphere : MonoBehaviour
{
    public float gravityStrength = 10f; // Strength of the gravity towards the center of the sphere

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                Vector3 directionToCenter = (transform.position - other.transform.position).normalized;
                playerRb.AddForce(directionToCenter * gravityStrength, ForceMode.Acceleration);
            }
        }
    }
}
