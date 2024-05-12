using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeRemaining = 60.0f;
    private bool timerIsRunning = false;

    private void Start()
    {
        // Set the initial time to display
        DisplayTime(timeRemaining);
        // Start the timer
        timerIsRunning = true;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            // Update the timer
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                // Timer has finished
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        // Convert the time to minutes and seconds
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Update the text UI
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
