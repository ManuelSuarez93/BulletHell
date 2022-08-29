using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] int _maxHealth;  
    private int _currentHealth; 
    public int CurrentHealth => _currentHealth;
    public float HealthPcnt => (float)_currentHealth/ _maxHealth;
    private void Awake() => _currentHealth = _maxHealth; 
    public void ChangeHealth(int health) => _currentHealth = health;
    public void RemoveHealth(int health) => _currentHealth -= health;
    public void AddHealth(int health) => _currentHealth += health;
    public void ResetHealth() => _currentHealth = _maxHealth;
}
