
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    [SerializeField] private float _interactionRange = 5f;
    [SerializeField] private int _numberOfPickups;
    [SerializeField] private float _sphereHeight = 0.62f;
    public TMP_Text scoreText;
    private int score;
    public LayerMask pickupLayerMask;
    public LayerMask wallLayer;
    public GameObject pickupPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
        SpawnSpheres();
    }

    private void SpawnSpheres()
    {
        for(int i =0; i < _numberOfPickups; i++)
        {
            Vector3 randomPosition = GetRandomPositionInSpawnArea();
            randomPosition = GetValidSpawnPosition(randomPosition);
            Instantiate(pickupPrefab, randomPosition, Quaternion.identity);
        }
    }

    private Vector3 GetValidSpawnPosition(Vector3 randomPosition)
    {
        while (IsPositionInsideWalls(randomPosition))
        {
            randomPosition = GetRandomPositionInSpawnArea();
        }
        return randomPosition;
    }

    private bool IsPositionInsideWalls(Vector3 randomPosition)
    {
        float sphereRadius = 1f;
        RaycastHit hit;
        return Physics.SphereCast(randomPosition, sphereRadius, Vector3.up, out hit, 0f, wallLayer);
    }

    private Vector3 GetRandomPositionInSpawnArea()
    {
        float randomX = Random.Range(-26f, 26f);
        float randomZ = Random.Range(-26f, 26f);
        return new Vector3(randomX, _sphereHeight, randomZ);
    }

    // Update is called once per frame
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
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _interactionRange, pickupLayerMask))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if(interactable != null)
                {
                    interactable.Interact();
                    IncreaseScore();
                }
            }
        }
    }
    public void IncreaseScore()
    {
        score += 1;
        UpdateScoreText();
    }
}

