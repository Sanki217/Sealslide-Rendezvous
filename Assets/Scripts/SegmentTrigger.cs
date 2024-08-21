using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentTrigger : MonoBehaviour
{
    public List<GameObject> segmentPrefabs;  // List of segment prefabs
    private List<GameObject> availableSegments;  // List to track available segments for spawning

    public float spawnZPosition = 120f;  // The Z position at which segments spawn
    public float speedIncrement = 0.5f;  // The amount by which the speed increases for each new segment

    private bool hasSpawned = false; // To ensure only one segment spawns

    private void Start()
    {
        // Initialize the available segments list with the segment prefabs
        availableSegments = new List<GameObject>(segmentPrefabs);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasSpawned && other.gameObject.CompareTag("Player"))
        {
            // Find the closest segment's movement component
            SegmentMovement currentSegmentMovement = FindObjectOfType<SegmentMovement>();

            if (currentSegmentMovement != null)
            {
                // Spawn a new segment from the available pool
                if (availableSegments.Count > 0)
                {
                    // Choose a random segment from the available list
                    int randomIndex = Random.Range(0, availableSegments.Count);
                    GameObject newSegmentPrefab = availableSegments[randomIndex];

                    // Instantiate the new segment at the desired position
                    GameObject newSegment = Instantiate(newSegmentPrefab, new Vector3(5, 3, spawnZPosition), Quaternion.identity);

                    // Get the SegmentMovement component from the newly created segment
                    SegmentMovement newSegmentMovement = newSegment.GetComponent<SegmentMovement>();

                    if (newSegmentMovement != null)
                    {
                        // Increase the speed by a small, consistent amount
                        float newSpeed = currentSegmentMovement.CurrentSpeed + speedIncrement;

                        // Set the new segment's speed to the calculated speed
                        newSegmentMovement.SetInitialSpeed(newSpeed);
                    }

                    // Remove the used segment from the available list
                    availableSegments.RemoveAt(randomIndex);

                    // If all segments have been used, refill the list
                    if (availableSegments.Count == 0)
                    {
                        availableSegments = new List<GameObject>(segmentPrefabs);
                    }

                    // Set hasSpawned to true to prevent additional spawning
                    hasSpawned = true;
                }
            }
            else
            {
                Debug.LogError("SegmentMovement component missing on the segment.");
            }
        }
    }
}
