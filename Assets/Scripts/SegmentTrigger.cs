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
            GameObject newSegment = Instantiate(Segment, new Vector3(0, 13, 777), Quaternion.identity);

            // Get the SegmentMovement component from the newly created segment
            SegmentMovement newSegmentMovement = newSegment.GetComponent<SegmentMovement>();

            // Get the SegmentMovement component from the current segment (the one triggering the spawn)
            SegmentMovement currentSegmentMovement = other.GetComponentInParent<SegmentMovement>();

            // If both segments have the SegmentMovement component, pass the current speed to the new segment
            if (newSegmentMovement != null && currentSegmentMovement != null)
            {
                newSegmentMovement.SetInitialSpeed(currentSegmentMovement.CurrentSpeed);
            }
        }
    }
}
