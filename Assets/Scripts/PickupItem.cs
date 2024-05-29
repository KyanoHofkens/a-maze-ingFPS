using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

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

    [SerializeField] private Image _crosshair;
    [SerializeField] private Sprite _interactButton;
    private Sprite _crosshairOriginal;
    private Vector2 _crosshairOriginalSize;

    [SerializeField] private AudioClip[] _pickupSoundClips;
    

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
        _crosshairOriginal = _crosshair.sprite;
        _crosshairOriginalSize = _crosshair.rectTransform.sizeDelta;

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
        ScoreText.text = "Food: " + Score.ToString();
        ScoreAboveHead.text = Score.ToString();
    }
    private void PickUpItem()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _interactionRange))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if(interactable != null)
            {
                _crosshair.sprite = _interactButton;
                _crosshair.rectTransform.sizeDelta = _crosshairOriginalSize * 5;
                _crosshair.color = Color.white;
            }
            else
            {
                _crosshair.sprite = _crosshairOriginal;
                _crosshair.rectTransform.sizeDelta = _crosshairOriginalSize;
                _crosshair.color = Color.red;
            }
            if (_playerInput.actions["Interact"].WasPressedThisFrame())
            {
                Debug.Log("got here");
                
                if (interactable != null)
                {
                    interactable.Interact();
                    _crosshair.sprite = _crosshairOriginal;
                    _crosshair.rectTransform.sizeDelta = _crosshairOriginalSize;
                    _crosshair.color = Color.red;
                    _pointsPickedUp++;
                    PointAnim.text = "+" + _pointsPickedUp.ToString();
                    StartCoroutine("IncreaseScore");
                    Debug.Log("picked up item");
                    SoundFxManager.Instance.PlayRandomSoundClip(_pickupSoundClips, this.transform, 1f);
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
        }
    }
}
