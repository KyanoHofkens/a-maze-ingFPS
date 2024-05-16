using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeRemaining = 60.0f;
    public bool timerIsRunning = false;

    private PlayerInput _playerInput;

    public bool _startTimer = false;

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
            StartCoroutine(StartTimer());

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
                    SceneManager.LoadScene("Leaderboard");
                }
            }
        }
    }

    IEnumerator StartTimer()
    {
        if (_startTimer)
        {
            Debug.Log("Start pressed");
            yield return new WaitForSeconds(5f);
            timerIsRunning = true;
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
