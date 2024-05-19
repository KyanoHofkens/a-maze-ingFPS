using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using StarterAssets;

public class PlayerManager : MonoBehaviour
{
    public List<PlayerInput> _players = new List<PlayerInput>();
    [SerializeField]
    private List<Transform> _startingPoints;
    [SerializeField]
    private List<LayerMask> _playerLayers;

    private PlayerInputManager _playerInputManager;
    public int PlayerCount = 0;

    private void Awake()
    {
        if(_playerInputManager == null)
        {
            _playerInputManager = FindObjectOfType<PlayerInputManager>();
        }
    }

    private void OnEnable()
    {
        _playerInputManager.onPlayerJoined += AddPlayer;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        _playerInputManager.onPlayerJoined -= AddPlayer;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void AddPlayer(PlayerInput _player)
    {
        _players.Add(_player);
        Debug.Log(_players.Count);

        _player.gameObject.GetComponent<CharacterController>().enabled = false;

        Transform _playerParent = _player.transform.parent;
        _playerParent.position = _startingPoints[_players.Count - 1].position;

        _player.gameObject.GetComponent<CharacterController>().enabled = true;

        //convert layer mask (bit) to an integer
        int _layerToAdd = (int)Mathf.Log(_playerLayers[_players.Count - 1].value, 2);

        //set the layer
        _playerParent.gameObject.layer = _layerToAdd;
        _playerParent.GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = _layerToAdd;
        //add the layers
        _playerParent.GetComponentInChildren<Camera>().cullingMask |= 1 << _layerToAdd;
    }

    private void TeleportExistingPlayers()
    {
        for(int i = 0; i< _players.Count; i++)
        {
            _players[i].gameObject.GetComponent<CharacterController>().enabled = false;

            Transform _playerParent = _players[i].transform.parent;
            _playerParent.position = _startingPoints[i].position;

            _players[i].gameObject.GetComponent<CharacterController>().enabled = true;
        }
    }

    private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        Debug.Log(_scene.name);
        if (_scene.name == "MainGameRepeat")
        {
            TeleportExistingPlayers();
        }
    }
}
