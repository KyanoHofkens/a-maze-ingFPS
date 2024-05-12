using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootController : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] GameObject _player;

    private bool _inArena = false;  

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        _playerInput = GetComponent<PlayerInput>();   
    }

    // Update is called once per frame
    void Update()
    {
       
        if (_playerInput.actions["Fire"].WasPressedThisFrame()) 
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        // Get the center of the screen
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Shoot a raycast from the center of the camera
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        // Check if the raycast hits something
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the hit object has the "Player" tag
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Hit player!");
                // Handle the collision with the player
                // Get the Player component from the hit object
                GameObject hitPlayer = hit.collider.gameObject;

                //teleport naar arena
                if (!_inArena)
                {                    
                    _inArena = true;
                    hitPlayer.transform.position = new Vector3(60,0.9f,16);

                    _player.transform.position = new Vector3(60, 0.9f, -16);
                }
                //damage doen in arena
                if(_inArena) 
                { 
                
                
                }
            }
            
        }
    }
}
