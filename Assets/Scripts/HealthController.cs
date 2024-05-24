using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private Material _fullHealthMaterial;
    [SerializeField] private Material _halfHealthMaterial;
    [SerializeField] private Material _lowHealthMaterial;

    [SerializeField] private GameObject _capsule;

    private int _maxHealth = 3;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        ChangeColorBasedOnHealth();
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
}
