using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] int _maxHealth; 
    [SerializeField] Image _img;
    private int _currentHealth;
    private void Update()
    {
        if (_img != null)
            _img.fillAmount = (float)_currentHealth / _maxHealth;
    }
    public int CurrentHealth => _currentHealth;
    private void Start() => _currentHealth = _maxHealth; 
    public void ChangeHealth(int health) => _currentHealth = health;
    public void RemoveHealth(int health) => _currentHealth -= health;
    public void AddHealth(int health) => _currentHealth += health;
    public void ResetHealth() => _currentHealth = _maxHealth;
}
