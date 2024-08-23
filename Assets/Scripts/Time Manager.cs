using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowDuration = 2f;   // Duration of the slow-motion effect
    public AnimationCurve timeScaleCurve;  // Animation curve for time scale over slow motion
    private bool isSlowing = false;   // To check if the slow-motion effect is active

    private float startTimeScale;
    private float startFixedDeltaTime;

    [Header("Debug Info")]
    public float currentTimeScale;  // To display the current time scale in the inspector

    void Start()
    {
        startTimeScale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Update()
    {
        // Update the current time scale for debugging purposes
        currentTimeScale = Time.timeScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TimeSlower"))  // Tag for objects that trigger slow-motion
        {
            if (!isSlowing)
            {
                StartCoroutine(SlowTime());
            }
        }
    }

    private IEnumerator SlowTime()
    {
        isSlowing = true;
        float elapsedTime = 0f;

        while (elapsedTime < slowDuration)
        {
            // Evaluate the time scale directly from the curve
            float curveValue = timeScaleCurve.Evaluate(elapsedTime / slowDuration);

            // Set Time.timeScale directly to the curve value
            Time.timeScale = curveValue;
            Time.fixedDeltaTime = startFixedDeltaTime * Time.timeScale;

            // Debug log to monitor curve value and time scale
            Debug.Log($"Curve Value: {curveValue}, Time Scale: {Time.timeScale}");

            elapsedTime += Time.unscaledDeltaTime;  // Use unscaled time to avoid affecting duration
            currentTimeScale = Time.timeScale;

            yield return null;
        }

        Time.timeScale = startTimeScale;  // Reset time scale to normal
        Time.fixedDeltaTime = startFixedDeltaTime;
        isSlowing = false;
        currentTimeScale = Time.timeScale;
    }

}
