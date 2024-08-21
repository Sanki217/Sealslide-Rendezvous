using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float TimeScale;
    public float StartTimeScale;
    public float StartFixedDeltaTime;

    void Start()
    {
        StartTimeScale = Time.timeScale;
        StartFixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
          StartSlowMotion();
        }

        else
        {
            StopSlowMotion();
        }
    }

    public void StartSlowMotion()
    {
        Time.timeScale = TimeScale;
        Time.fixedDeltaTime = StartFixedDeltaTime * TimeScale;
    }

    public void StopSlowMotion()
    {
        Time.timeScale = StartTimeScale;
        Time.fixedDeltaTime = StartFixedDeltaTime;
    }
    
}
