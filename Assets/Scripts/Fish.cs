using UnityEngine;
using TMPro;

public class Fish : MonoBehaviour
{
    public float maxMoveSpeed = 5f;      // Maximum speed at which fish move to the collection point
    public AnimationCurve speedCurve;    // Curve to control speed
    public AnimationCurve sizeCurve;     // Curve to control size
    public float collectionTime = 2f;    // Time it takes to reach the collection point
    private bool isCollected = false;    // Whether the fish has been collected

    private Rigidbody rb;                // Rigidbody of the fish
    private Vector3 initialScale;        // Initial size of the fish
    private float collectionStartTime;   // Time when collection starts

    private Transform playerMouth;       // Reference to the player's mouth
    private TextMeshProUGUI scoreText;   // Reference to the score text in the UI

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialScale = transform.localScale;

        // Get references from the GameManager
        playerMouth = GameManager.Instance.collectionPoint;  // Assuming collectionPoint is set to the mouth position
        scoreText = GameManager.Instance.scoreText;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCollected = true;
            rb.isKinematic = true;  // Disable physics to smoothly move the fish
            transform.SetParent(other.transform);  // Parent the fish to the player
            collectionStartTime = Time.time;  // Set the time when the collection started
        }
    }

    void Update()
    {
        if (isCollected)
        {
            float elapsedTime = Time.time - collectionStartTime;  // Time since the fish was collected
            float t = Mathf.Clamp01(elapsedTime / collectionTime);  // Normalize the elapsed time

            // Adjust the movement speed and size based on curves over the entire collection time
            float speedMultiplier = speedCurve.Evaluate(t);
            float sizeMultiplier = sizeCurve.Evaluate(t);

            // Smoothly move fish towards the player's mouth using Lerp
            transform.position = Vector3.Lerp(transform.position, playerMouth.position, maxMoveSpeed * speedMultiplier * Time.deltaTime);

            // Adjust size over time using the size curve
            transform.localScale = Vector3.Lerp(transform.localScale, initialScale * sizeMultiplier, t);

            // If the fish is close enough to the player's mouth, "collect" it
            if (Vector3.Distance(transform.position, playerMouth.position) < 0.1f)
            {
                CollectFish();
            }
        }
    }

    void CollectFish()
    {
        // Increment score and update the UI
        GameManager.Instance.IncreaseScore(1);

        // Destroy the fish object
        Destroy(gameObject);
    }
}
