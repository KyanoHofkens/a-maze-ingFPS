using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    [SerializeField] private float _interactionRange = 5f;
    [SerializeField] private int _numberOfPickups;
    [SerializeField] private float _sphereHeight = 0.62f;
    public LayerMask pickupLayerMask;
    public LayerMask wallLayer;
    public GameObject pickupPrefab;
    public List<PickupItem> Score = new List<PickupItem>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnSpheres();
    }
    private void Update()
    {

    }
    public void AddScore(PickupItem pickUpItem)
    {
        Score.Add(pickUpItem);
    }
    private void Awake()
    {
        DontDestroyOnLoad(this);
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
}

