using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ReadyCheck : MonoBehaviour
{
    public TMP_Text Countdown;
    public TMP_Text Player1;
    public TMP_Text Player2;
    public TMP_Text Player3;
    public TMP_Text Player4;
    public TMP_Text PressStart;
    public Camera Cam;
    private float _countdownDuration = 5f;
    private PlayerManager _playerManager;
    private List<FirstPersonController> _firstPersonController = new List<FirstPersonController>();
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InitiateMenu();
        _playerManager = FindAnyObjectByType<PlayerManager>();
    }

    private void InitiateMenu()
    {
        Player1.text = "Player 1: not joined";
        Player2.text = "Player 2: not joined";
        Player3.text = "Player 3: not joined";
        Player4.text = "Player 4: not joined";
        Countdown.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        Player1Ready();
        Player2Ready();
        Player3Ready();
        Player4Ready();
        CountdownCheck();
    }

    private void CountdownCheck()
    {
        if(_playerManager._players.Count >=2 && _playerManager._players[0].actions["Start"].WasPressedThisFrame())
        {
            StartCoroutine(StartCountdown());
                
        }
    }
    IEnumerator StartCountdown()
    {
        float currentTime = _countdownDuration;
        while(currentTime > 0 )
        {
            Countdown.text = currentTime.ToString();
            yield return new WaitForSeconds(1f);
            currentTime--;
        }
        Countdown.text = "0";
        if(currentTime == 0)
        {
            Countdown.gameObject.SetActive(false);
            Player1.gameObject.SetActive(false);
            Player2.gameObject.SetActive(false);
            Player3.gameObject.SetActive(false);
            Player4.gameObject.SetActive(false);
            PressStart.gameObject.SetActive(false);
            foreach (FirstPersonController firstPersonController in _firstPersonController)
                firstPersonController._canMove = true;
        }
    }
    public void RegisterPlayer(FirstPersonController controller)
    {
        _firstPersonController.Add(controller);
    }
    private void Player1Ready()
    {
        if (_playerManager._players.Count >= 1)
        {
            Cam.gameObject.SetActive(false);
            Player1.text = "Player 1: Joined";
        }
    }

    private void Player2Ready()
    {
        if (_playerManager._players.Count >=2)
        {
            Player2.text = "Player 2: Joined";
        }
    }

    private void Player3Ready()
    {
        if (_playerManager._players.Count >= 3)
        {
            Player3.text = "Player 3: Joined";
        }
    }

    private void Player4Ready()
    {
        if (_playerManager._players.Count == 4)
        {
            Player4.text = "Player 4: Joined";
        }
    }
}
