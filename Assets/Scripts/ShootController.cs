using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject _arena;

    private PlayerInput _playerInput;

    private bool _inArena = false;  

    // Start is called before the first frame update
    void Start()
    {
        _inArena = false;
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
        Vector3 viewPortCenter = new Vector3(.5f, .5f, 100);

        // Shoot a raycast from the center of the camera
        Ray ray = mainCamera.ViewportPointToRay(viewPortCenter);
        RaycastHit hit;

        // Check if the raycast hits something
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 100.0f))
        {
            // Check if the hit object has the "Player" tag
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Hit player!");
                // Handle the collision with the player
                // Get the Player component from the hit object
                GameObject hitPlayer = hit.collider.gameObject;
                Debug.Log(hitPlayer.name);
                //teleport naar arena
                if (!_inArena)
                {
                    _inArena = true;

                    hitPlayer.GetComponent<CharacterController>().enabled = false;
                    hitPlayer.transform.position = new Vector3(0.079f, 0.615f, -38.32f);

                    this.gameObject.GetComponent<CharacterController>().enabled = false;
                    this.transform.position = new Vector3(0.079f, 0.615f, -58.5f);

                    hitPlayer.GetComponentInParent<CharacterController>().enabled = true;
                    this.gameObject.GetComponent<CharacterController>().enabled = true;
                }
                //damage doen in arena
                if (_inArena)
                {


                }
            }
            
        }
    }
}
