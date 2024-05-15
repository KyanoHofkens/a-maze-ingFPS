using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private float _interactionRange = 5f;
    [SerializeField] private Camera _camera;
    public TMP_Text scoreText;
    private int score;
    public LayerMask pickupLayerMask;

    private PlayerInput _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
        _playerInput = GetComponent<PlayerInput>();
    }
    void Update()
    {
        PickUpItem();
    }
    private void UpdateScoreText()
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
}
