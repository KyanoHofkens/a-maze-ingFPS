using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeRemaining = 60.0f;
    private bool timerIsRunning = false;

    private PlayerInput _playerInput;

    private void Start()
    {
        // Set the initial time to display
        DisplayTime(timeRemaining);
    }

    private void Update()
    {
        if(_playerInput == null)
        {
            _playerInput = GameObject.FindObjectOfType<PlayerInput>();
        }

        if(_playerInput != null)
        {
            if (_playerInput.actions["Start"].WasPressedThisFrame())
            {
                Debug.Log("Start pressed");
                timerIsRunning = true;
            }

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
                    Application.Quit();
                }
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
