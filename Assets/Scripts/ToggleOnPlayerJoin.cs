using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleOnPlayerJoin : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;

    private void Awake()
    {
        _playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        _playerInputManager.onPlayerJoined += ToggleThis;
    }

    private void OnDisable()
    {
        _playerInputManager.onPlayerLeft -= ToggleThis;
    }

    private void ToggleThis(PlayerInput _player)
    {
        this.gameObject.SetActive(false);
    }
}
