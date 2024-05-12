using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerInput> _players = new List<PlayerInput>();
    [SerializeField]
    private List<Transform> _startingPoints;
    [SerializeField]
    private List<LayerMask> _playerLayers;

    private PlayerInputManager _playerInputManager;

    private void Awake()
    {
        _playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        _playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        _playerInputManager.onPlayerJoined -= AddPlayer;
    }

    public void AddPlayer(PlayerInput _player)
    {
        _players.Add(_player);

        _player.gameObject.GetComponent<CharacterController>().enabled = false;

        Transform _playerParent = _player.transform.parent;
        _playerParent.position = _startingPoints[_players.Count - 1].position;

        _player.gameObject.GetComponent<CharacterController>().enabled = true;

        //convert layer mask (bit) to an integer
        int _layerToAdd = (int)Mathf.Log(_playerLayers[_players.Count - 1].value, 2);

        //set the layer
        _playerParent.GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = _layerToAdd;
        //add the layers
        _playerParent.GetComponentInChildren<Camera>().cullingMask |= 1 << _layerToAdd;
    }
}
