using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleOnStart: MonoBehaviour
{
    private PlayerInput _playerInput;

    private void Update()
    {
        if (_playerInput == null)
        {
            _playerInput = GameObject.FindObjectOfType<PlayerInput>();
        }

        if (_playerInput != null)
        {
            if (_playerInput.actions["Start"].WasPressedThisFrame())
            {
                ToggleThis();
            }
        }
    }
    private void ToggleThis()
    {
        this.gameObject.SetActive(false);
    }
}
