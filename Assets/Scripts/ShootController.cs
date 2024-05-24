using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShootController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject _arena;
    private PlayerInput _playerInput;
    public PickupItem PickupItem;

    [SerializeField] private LineRenderer _lineRend;
    [SerializeField] private Transform _gunAim;
    [SerializeField] private  Image _crosshair;
    public TMP_Text ScoreDifference;
    public RectTransform ScoreTransform;

    private float _liveTimer = 0f;
    private float _liveTime = .1f;

    [SerializeField] private Sprite _hitmarkerSprite;
    [SerializeField] private Sprite _R2Sprite;
    private Sprite _crosshairSprite;
    private float _crosshairTimer = 0f;
    private float _crosshairDelay = .3f;
    private bool _crosshairChanged = false;
    private Vector2 _crosshairOriginalSize;

    public TMP_Text FightText;
    private float fightTimer=0;
    public GameObject FightTextObject;

    public bool _inArena = false;

    // Start is called before the first frame update
    void Start()
    {
        _inArena = false;
        _playerInput = GetComponent<PlayerInput>();
        _crosshairSprite = _crosshair.sprite;
        _crosshairOriginalSize = _crosshair.rectTransform.sizeDelta;
        FightText.text = "Fight";

        FightTextObject.SetActive(false);
    }

    private void Awake()
    {
        _inArena = false;
        _playerInput = GetComponent<PlayerInput>();
        PickupItem = FindObjectOfType<PickupItem>();
        ScoreDifference.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.z < -28) { _inArena = true; }
        if (this.transform.position.z > -28) { _inArena = false; }
        ArenaChecker();

        if(!_crosshairChanged)
        {
            LookingAtEnemyCheck();
        }

        if (_playerInput.actions["Fire"].WasPressedThisFrame())
        {
            Shoot();            
        }

        if(_crosshairChanged)
        {
            _crosshairTimer += Time.deltaTime;

            if(_crosshairTimer >= _crosshairDelay)
            {
                _crosshair.rectTransform.sizeDelta = _crosshairOriginalSize;
                _crosshair.sprite = _crosshairSprite;
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

    private void ArenaChecker()
    {
        
        if(_inArena & fightTimer <= 1) 
        {            
            FightTextObject.SetActive(true);
            fightTimer += Time.deltaTime;
        }
        if(fightTimer > 1) 
        { 
        FightTextObject.SetActive(false);
        }       

        if(!_inArena & fightTimer > 0) 
        {
            fightTimer = 0;
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
                if(hit.collider.gameObject != this.gameObject)
                {
                    _crosshair.rectTransform.sizeDelta = _crosshairOriginalSize * 10;
                    _crosshair.sprite = _hitmarkerSprite;
                    _crosshairChanged = true;

                    // Handle the collision with the player
                    // Get the Player component from the hit object
                    GameObject hitPlayer = hit.collider.gameObject;
                    Debug.Log(hitPlayer.name);
                    //teleport naar arena
                    if (!_inArena)
                    {
                        hitPlayer.GetComponent<CharacterController>().enabled = false;
                        hitPlayer.transform.position = new Vector3(0.079f, 0.615f, -61.62f);
                        hitPlayer.transform.rotation = Quaternion.Euler(0, 0, 0);

                        this.gameObject.GetComponent<CharacterController>().enabled = false;
                        this.transform.position = new Vector3(0.079f, 0.615f, -33.92f);
                        this.transform.rotation = Quaternion.Euler(0, 180, 0);

                        hitPlayer.GetComponentInParent<CharacterController>().enabled = true;
                        this.gameObject.GetComponent<CharacterController>().enabled = true;

                        // toggle the hearts to visible
                        this.GetComponent<HealthController>().ToggleHearts();
                        hitPlayer.GetComponent<HealthController>().ToggleHearts();
                    }
                    //damage doen in arena
                    if (_inArena)
                    {
                        HealthController hitHealthController = hitPlayer.GetComponent<HealthController>();
                        hitHealthController.TakeDamage(1);
                        if(hitHealthController.GetHealth() <= 0) 
                        {
                            int score = hitPlayer.GetComponent<PickupItem>().Score / 2;

                            this.gameObject.GetComponent<CharacterController>().enabled = false;
                            this.transform.position = new Vector3(1.06f, 0.615f, -23.16f);

                            hitPlayer.GetComponent<CharacterController>().enabled = false;
                            hitPlayer.transform.position = new Vector3(1.06f, 0.615f, 25.28f);

                            hitPlayer.GetComponentInParent<CharacterController>().enabled = true;
                            this.gameObject.GetComponent<CharacterController>().enabled = true;

                            this.ShowPointChange(score, true);
                            hitPlayer.GetComponent<ShootController>().ShowPointChange(score, false);
                            this.GetComponent<PickupItem>().Score += score;
                            //this.GetComponent<PickupItem>().UpdateScoreText();
                            hitPlayer.GetComponent<PickupItem>().Score -= score;
                            //hitPlayer.GetComponent<PickupItem>().UpdateScoreText();

                            //reset player's health
                            this.GetComponent<HealthController>().ResetHealth();
                            hitHealthController.ResetHealth();

                            // toggle hearts to invisible
                            this.GetComponent<HealthController>().ToggleHearts();
                            hitHealthController.ToggleHearts();
                        }
                    }
                }
            }
        }
    }
    public void ShowPointChange(int score, bool isWinner)
    {
        ScoreDifference.text = (isWinner ? "+" : "-") + score.ToString();
        ScoreDifference.color = isWinner ? Color.green : Color.red;
        StartCoroutine("MoveScore");
    }
    IEnumerator MoveScore()
    {
        Vector2 startPosition = ScoreDifference.rectTransform.anchoredPosition;
        Vector2 endPosition = ScoreTransform.anchoredPosition;
        float duration = 1.5f;
        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            ScoreDifference.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, elapsedTime / duration);
            yield return null;
        }
        ScoreDifference.rectTransform.anchoredPosition = startPosition;
        ScoreDifference.text = "";
        GetComponent<PickupItem>().UpdateScoreText();
    }

    private void LookingAtEnemyCheck()
    {
        Vector3 viewPortCenter = new Vector3(.5f, .5f, 100);

        Ray ray = mainCamera.ViewportPointToRay(viewPortCenter);
        RaycastHit hit;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 100.0f))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (hit.collider.gameObject != this.gameObject)
                {
                    _crosshair.color = Color.white;
                    _crosshair.rectTransform.sizeDelta = _crosshairOriginalSize * 5;
                    _crosshair.sprite = _R2Sprite;

                }
            }
            else if (!_crosshairChanged)
            {
                _crosshair.color = Color.red;
                _crosshair.rectTransform.sizeDelta = _crosshairOriginalSize;
                _crosshair.sprite = _crosshairSprite;
            }
        }
    }
}
