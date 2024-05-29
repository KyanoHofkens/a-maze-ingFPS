using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private Material _fullHealthMaterial;
    [SerializeField] private Material _halfHealthMaterial;
    [SerializeField] private Material _lowHealthMaterial;

    [SerializeField] private GameObject _capsule;

    [SerializeField] private GameObject _hearts;
    [SerializeField] private Image _heart1;
    [SerializeField] private Image _heart2;
    [SerializeField] private Image _heart3;
    [SerializeField] private Sprite _fullHeart;
    [SerializeField] private Sprite _emptyHeart;

    private int _maxHealth = 3;
    private int _currentHealth;

    [SerializeField] private AudioClip[] _hurtSoundClips;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
        ChangeColorBasedOnHealth();
        ChangeAmountOfHearts();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        ChangeColorBasedOnHealth();
        ChangeAmountOfHearts();
        SoundFxManager.Instance.PlayRandomSoundClip(_hurtSoundClips, this.transform, 1f);
    }

    public int GetHealth() { return _currentHealth; }

    // change color of the character depending on current health
    private void ChangeColorBasedOnHealth()
    {
        int _thirdOfHealth = _maxHealth / 3;
        if(_currentHealth > 2 * _thirdOfHealth)
        {
            _capsule.GetComponent<Renderer>().material = _fullHealthMaterial;
            Debug.Log("blue");
        } else if (_currentHealth <= 2 * _thirdOfHealth && _currentHealth > _thirdOfHealth)
        {
            Debug.Log("yellow");
            _capsule.GetComponent<Renderer>().material = _halfHealthMaterial;
        } else
        {
            Debug.Log("red");
            _capsule.GetComponent<Renderer>().material = _lowHealthMaterial;
        }
    }

    private void ChangeAmountOfHearts()
    {
        switch (_currentHealth)
        {
            case 3:
                _heart1.sprite = _fullHeart;
                _heart2.sprite = _fullHeart;
                _heart3.sprite = _fullHeart;
                break;
            case 2:
                _heart1.sprite = _emptyHeart;
                break;
            case 1:
                _heart1.sprite = _emptyHeart;
                _heart2.sprite = _emptyHeart;
                break;
            case 0:
                _heart1.sprite = _emptyHeart;
                _heart2.sprite = _emptyHeart;
                _heart3.sprite = _emptyHeart;
                break;
        }
    }

    public void ToggleHearts()
    {
        _hearts.SetActive(!_hearts.activeSelf);
    }
}
