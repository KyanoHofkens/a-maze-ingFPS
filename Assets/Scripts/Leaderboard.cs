using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.SceneManagement;
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
        List<Tuple<int, string>> sortedData = new List<Tuple<int, string>>();
        for(int i = 0; i < _playerNames.Count; i++)
        {
            sortedData.Add(new Tuple<int, string>(_pickups.Score[i].Score, _playerNames[i]));
        }
        sortedData.Sort((x, y) => y.Item1.CompareTo(x.Item1));
        for(int i = 0; i< _playerNames.Count; i++)
        {
            _pickups.Score[i].Score = sortedData[i].Item1;
            _playerNames[i] = sortedData[i].Item2;
        }
        if(_pickups.Score.Count == 2)
        {
            FirstPlace.text = "1. " + _playerNames[0] + ": " + _pickups.Score[0].Score.ToString();
            SecondPlace.text = "2. " + _playerNames[1] + ": " + _pickups.Score[1].Score.ToString();
        }
        if(_pickups.Score.Count == 3)
        {
            FirstPlace.text = "1. " + _playerNames[0] + ": " + _pickups.Score[0].Score.ToString();
            SecondPlace.text = "2. " + _playerNames[1] + ": " + _pickups.Score[1].Score.ToString();
            ThirdPlace.text = "3. " + _playerNames[2] + ": " + _pickups.Score[2].Score.ToString();
        }
        if (_pickups.Score.Count == 4)
        {
            FirstPlace.text = "1. " + _playerNames[0] + ": " + _pickups.Score[0].Score.ToString();
            SecondPlace.text = "2. " + _playerNames[1] + ": " + _pickups.Score[1].Score.ToString();
            ThirdPlace.text = "3. " + _playerNames[2] + ": " + _pickups.Score[2].Score.ToString();
            FourthPlace.text = "4. " + _playerNames[3] + ": " + _pickups.Score[3].Score.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Restart();
    }

    private void Restart()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //EditorSceneManager.LoadScene("Assets/Scenes/MainGameRepeat.unity", LoadSceneMode.Single);
            SceneManager.LoadScene("MainGameRepeat");
        }
    }
}
