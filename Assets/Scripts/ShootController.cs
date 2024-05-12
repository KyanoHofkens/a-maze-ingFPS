using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject _arena;
    private int _hits = 0;
    private int _score = 0;
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
        if (this.transform.position.z < -28) { _inArena = true; }
        if (this.transform.position.z > -28) { _inArena = false; }

        PickUpItem();
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
                    hitPlayer.GetComponent<CharacterController>().enabled = false;
                    hitPlayer.transform.position = new Vector3(0.079f, 0.615f, -58.62f);

                    this.gameObject.GetComponent<CharacterController>().enabled = false;
                    this.transform.position = new Vector3(0.079f, 0.615f, -36.92f);

                    hitPlayer.GetComponentInParent<CharacterController>().enabled = true;
                    this.gameObject.GetComponent<CharacterController>().enabled = true;
                }
                //damage doen in arena
                if (_inArena)
                {
                    Debug.Log("Hit in arena");
                    _hits++;
                    if (_hits == 3) 
                    {
                        Debug.Log("3");
                        _hits = 0;

                        this.gameObject.GetComponent<CharacterController>().enabled = false;
                        this.transform.position = new Vector3(1.06f, 0.615f, -23.16f);

                        hitPlayer.GetComponent<CharacterController>().enabled = false;
                        hitPlayer.transform.position = new Vector3(1.06f, 0.615f, 25.28f);

                        hitPlayer.GetComponentInParent<CharacterController>().enabled = true;
                        this.gameObject.GetComponent<CharacterController>().enabled = true;
                    }
                }
            }

        }
    }
    private void PickUpItem()
    {
        if (_playerInput.actions["Interact"].WasPressedThisFrame())
        {
            Vector3 viewPortCenter = new Vector3(.5f, .5f, 100);

            Ray ray = mainCamera.ViewportPointToRay(viewPortCenter);
            RaycastHit hit;

            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 5.0f))
            {
                if ((hit.collider.CompareTag("Pickup")))
                { 
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                         interactable.Interact();
                        _score++;
                    }

                }
                
            }
        }
    }
}
