using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    public TMP_Text FirstPlace;
    public TMP_Text SecondPlace;
    public TMP_Text ThirdPlace;
    public TMP_Text FourthPlace;
    private Pickups _pickups;
    private List<string> _playerNames = new List<string>();
    private PlayerInput _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        _pickups = FindAnyObjectByType<Pickups>();
        for(int i = 0; i<_pickups.Score.Count; i++)
        {
            _playerNames.Add("Player " + (i + 1).ToString());
        }
        _playerInput = FindObjectOfType<PlayerInput>();
        InitiateLeaderboard();
    }

    private void InitiateLeaderboard()
    {
        //if(_pickups.Score.Count == 2)
        //{
        //    if (_pickups.Score[0].score > _pickups.Score[1].score)
        //    {
        //        FirstPlace.text = "1. Player 1: " + _pickups.Score[0].score.ToString();
        //        SecondPlace.text = "2. Player 2: " + _pickups.Score[1].score.ToString();
        //    }
        //    else if (_pickups.Score[0].score == _pickups.Score[1].score)
        //        SecondPlace.text = "It's a Draw!";
        //    else
        //    {
        //        FirstPlace.text = "1. Player 2: " + _pickups.Score[1].score.ToString();
        //        SecondPlace.text = "2. Player 1: " + _pickups.Score[0].score.ToString();
        //    }
        //}
        //if(_pickups.Score.Count == 3)
        //{
        //    int 
        //}
        List<Tuple<int, string>> sortedData = new List<Tuple<int, string>>();
        for(int i = 0; i < _playerNames.Count; i++)
        {
            sortedData.Add(new Tuple<int, string>(_pickups.Score[i].score, _playerNames[i]));
        }
        sortedData.Sort((x, y) => y.Item1.CompareTo(x.Item1));
        for(int i = 0; i< _playerNames.Count; i++)
        {
            _pickups.Score[i].score = sortedData[i].Item1;
            _playerNames[i] = sortedData[i].Item2;
        }
        if(_pickups.Score.Count == 2)
        {
            FirstPlace.text = "1. " + _playerNames[0] + ": " + _pickups.Score[0].score.ToString();
            SecondPlace.text = "2. " + _playerNames[1] + ": " + _pickups.Score[1].score.ToString();
        }
        if(_pickups.Score.Count == 3)
        {
            FirstPlace.text = "1. " + _playerNames[0] + ": " + _pickups.Score[0].score.ToString();
            SecondPlace.text = "2. " + _playerNames[1] + ": " + _pickups.Score[1].score.ToString();
            ThirdPlace.text = "3. " + _playerNames[2] + ": " + _pickups.Score[2].score.ToString();
        }
        if (_pickups.Score.Count == 4)
        {
            FirstPlace.text = "1. " + _playerNames[0] + ": " + _pickups.Score[0].score.ToString();
            SecondPlace.text = "2. " + _playerNames[1] + ": " + _pickups.Score[1].score.ToString();
            ThirdPlace.text = "3. " + _playerNames[2] + ": " + _pickups.Score[2].score.ToString();
            FourthPlace.text = "4. " + _playerNames[3] + ": " + _pickups.Score[3].score.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Restart();
    }

    private void Restart()
    {
        if (_playerInput.actions["Start"].WasPressedThisFrame())
        {
            SceneManager.LoadScene("MainGame");
        }
    }
}
