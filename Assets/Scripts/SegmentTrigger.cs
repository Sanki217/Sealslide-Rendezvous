using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentTrigger : MonoBehaviour
{
    public GameObject Segment;  // Reference to the segment prefab

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SegmentTrigger"))
        {
            // Instantiate the new segment at the desired position
            GameObject newSegment = Instantiate(Segment, new Vector3(5, 3, 150), Quaternion.identity);

            // Get the SegmentMovement component from the newly created segment
            SegmentMovement newSegmentMovement = newSegment.GetComponent<SegmentMovement>();

            // Get the SegmentMovement component from the current segment (the one triggering the spawn)
            SegmentMovement currentSegmentMovement = other.GetComponent<SegmentMovement>();

            // If both segments have the SegmentMovement component, calculate the speed for the new segment
            if (newSegmentMovement != null && currentSegmentMovement != null)
            {
                // Calculate the difference between the initial speed and the current speed of the previous segment
                float speedDifference = currentSegmentMovement.CurrentSpeed - currentSegmentMovement.initialSpeed;

                // Set the new segment's speed to the current speed of the previous segment
                newSegmentMovement.SetInitialSpeed(currentSegmentMovement.CurrentSpeed + speedDifference);
            }
        }
    }
}
