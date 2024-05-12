using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    public TMP_Text FirstPlace;
    public TMP_Text SecondPlace;
    public TMP_Text ThirdPlace;
    public TMP_Text FourthPlace;
    // Start is called before the first frame update
    void Start()
    {
        InitiateLeaderboard();
    }

    private void InitiateLeaderboard()
    {
        FirstPlace.text = "1. ";
        SecondPlace.text = "2. ";
        ThirdPlace.text = "3. ";
        FourthPlace.text = "4. ";
    }

    // Update is called once per frame
    void Update()
    {
        Restart();
    }

    private void Restart()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            SceneManager.LoadScene("StartScreen");
        }
    }
}
