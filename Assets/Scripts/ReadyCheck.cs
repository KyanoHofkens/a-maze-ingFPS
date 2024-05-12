using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyCheck : MonoBehaviour
{
    public TMP_Text Timer;
    public TMP_Text Player1;
    public TMP_Text Player2;
    public TMP_Text Player3;
    public TMP_Text Player4;
    private float _countdownDuration = 5f;
    private int _playerCount = 0;
    private float _countdownTimer;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InitiateMenu();
    }

    private void InitiateMenu()
    {
        Player1.text = "Player 1:";
        Player2.text = "Player 2:";
        Player3.text = "Player 3:";
        Player4.text = "Player 4:";
        Timer.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        Player1Ready();
        Player2Ready();
        Player3Ready();
        Player4Ready();
        StartCountdown();
    }

    private void StartCountdown()
    {
        if(_playerCount == 4)
        {
            _countdownTimer -= Time.deltaTime;
            Timer.text = Mathf.CeilToInt(_countdownTimer).ToString();
            if (_countdownTimer <= 0)
                StartGame();
        }
    }

    private void StartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    private void Player4Ready()
    {
        if (Input.GetKeyDown(KeyCode.Joystick4Button1))
        {
            _playerCount++;
            Player1.text = "Player 4: Ready";
        }
    }

    private void Player3Ready()
    {
        if (Input.GetKeyDown(KeyCode.Joystick3Button1))
        {
            _playerCount++;
            Player1.text = "Player 3: Ready";
        }
    }

    private void Player2Ready()
    {
        if (Input.GetKeyDown(KeyCode.Joystick2Button1))
        {
            _playerCount++;
            Player1.text = "Player 2: Ready";
        }
    }

    private void Player1Ready()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            _playerCount++;
            Player1.text = "Player 1: Ready";
        }
    }
}
