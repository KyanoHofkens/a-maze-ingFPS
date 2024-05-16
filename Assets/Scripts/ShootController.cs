using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShootController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject _arena;
    public int _hits = 0;
    private PlayerInput _playerInput;
    public PickupItem _pickupItem;

    [SerializeField] private LineRenderer _lineRend;
    [SerializeField] private Transform _gunAim;
    [SerializeField] private  Image _crosshair;

    private float _liveTimer = 0f;
    private float _liveTime = .1f;

    private float _crosshairTimer = 0f;
    private float _crosshairDelay = .3f;
    private bool _crosshairChanged = false;


    private bool _inArena = false;

    // Start is called before the first frame update
    void Start()
    {
        _inArena = false;
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Awake()
    {
        _inArena = false;
        _playerInput = GetComponent<PlayerInput>();
        _pickupItem = FindObjectOfType<PickupItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.z < -28) { _inArena = true; }
        if (this.transform.position.z > -28) { _inArena = false; }

        if (_playerInput.actions["Fire"].WasPressedThisFrame())
        {
            Shoot();            
        }

        if(_crosshairChanged)
        {
            _crosshairTimer += Time.deltaTime;

            if(_crosshairTimer >= _crosshairDelay)
            {
                _crosshair.color = Color.red;
                _crosshairChanged = false;
                _crosshairTimer = 0f;
            }
        }

        if (_lineRend.enabled)
        {
            _liveTimer += Time.deltaTime;

            if( _liveTimer >= _liveTime)
            {
                Debug.Log("Disabled Laser");
                _lineRend.enabled = false;
            }
        }
    }
    private void Shoot()
    {
        _liveTimer = 0f;
        // Get the center of the screen
        Vector3 viewPortCenter = new Vector3(.5f, .5f, 100);

        // Shoot a raycast from the center of the camera
        Ray ray = mainCamera.ViewportPointToRay(viewPortCenter);
        RaycastHit hit;

        // Check if the raycast hits something
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 100.0f))
        {
            _lineRend.enabled = true;
            _lineRend.SetPosition(0, _gunAim.transform.position);
            _lineRend.SetPosition(1, hit.point);

            // Check if the hit object has the "Player" tag
            if (hit.collider.CompareTag("Player"))
            {
                _crosshair.color = Color.blue;
                _crosshairChanged = true;

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
                        hitPlayer.GetComponent<ShootController>()._hits = 0;

                        int score = hitPlayer.GetComponent<PickupItem>().score / 2;
                        this.GetComponent<PickupItem>().score += score;
                        hitPlayer.GetComponent<PickupItem>().score = score;
                        this.GetComponent<PickupItem>().UpdateScoreText();
                        hitPlayer.GetComponent<PickupItem>().UpdateScoreText();

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
}
