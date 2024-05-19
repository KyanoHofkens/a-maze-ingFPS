using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PickupItem : MonoBehaviour
{
    private float _interactionRange = 5f;
    [SerializeField] private Camera _camera;
    public TMP_Text scoreText;
    public int score = 0;
    public LayerMask pickupLayerMask;
    private PlayerInput _playerInput;
    private Pickups _pickups;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        PickUpItem();
    }

    private void Awake()
    {
        UpdateScoreText();
        _playerInput = GetComponent<PlayerInput>();
        _pickups = FindAnyObjectByType<Pickups>();
        this.gameObject.SetActive(true);
        _pickups.AddScore(this);
    }
    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
    private void PickUpItem()
    {
        if (_playerInput.actions["Interact"].WasPressedThisFrame())
        {
            Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _interactionRange, pickupLayerMask))
            {
                Debug.Log("got here");
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.Interact();
                    this.IncreaseScore();
                    Debug.Log("picked up item");
                }
            }
        }
    }
    public void IncreaseScore()
    {
        Debug.Log("increased score");
        score += 1;
        UpdateScoreText();
    }

    private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        Debug.Log(_scene.name);
        if (_scene.name == "MainGameRepeat")
        {
            this.score = 0;
            UpdateScoreText();
            _playerInput = GetComponent<PlayerInput>();
            _pickups = FindAnyObjectByType<Pickups>();
            this.gameObject.SetActive(true);
            _pickups.AddScore(this);
        }
    }
}
