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
    public TMP_Text ScoreText;
    public TMP_Text ScoreAboveHead;
    public TMP_Text PointAnim;
    public RectTransform TargetTransform;
    public int Score = 0;
    public LayerMask pickupLayerMask;
    private PlayerInput _playerInput;
    private Pickups _pickups;
    private Vector2 _startPosition;
    private int _pointsPickedUp;

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
        _startPosition = PointAnim.rectTransform.anchoredPosition;
        _pointsPickedUp = 0;
    }
    public void UpdateScoreText()
    {
        ScoreText.text = "Score: " + Score.ToString();
        ScoreAboveHead.text = Score.ToString();
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
                    _pointsPickedUp++;
                    PointAnim.text = "+" + _pointsPickedUp.ToString();
                    StartCoroutine("IncreaseScore");
                    Debug.Log("picked up item");
                }
            }
        }
    }
    IEnumerator IncreaseScore()
    {
        Debug.Log("increased score");
        Vector2 endPosition = TargetTransform.anchoredPosition;
        float duration = 1f;
        float elapsedTime = 0;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            PointAnim.rectTransform.anchoredPosition = Vector2.Lerp(_startPosition, endPosition, elapsedTime / duration);
            yield return null;
        }
        PointAnim.rectTransform.anchoredPosition = _startPosition;
        PointAnim.text = "";
        Score += _pointsPickedUp;
        _pointsPickedUp = 0;
        UpdateScoreText();
    }

    private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        Debug.Log(_scene.name);
        if (_scene.name == "MainGameRepeat")
        {
            this.Score = 0;
            UpdateScoreText();
            _playerInput = GetComponent<PlayerInput>();
            _pickups = FindAnyObjectByType<Pickups>();
            this.gameObject.SetActive(true);
            _pickups.AddScore(this);
        }
    }
}
