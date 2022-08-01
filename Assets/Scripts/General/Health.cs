using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int _maxHealth;
    private int _currentHealth;

    public int CurrentHealth => _currentHealth;
    private void Start() => _currentHealth = _maxHealth;

    public void ChangeHealth(int health) => _currentHealth = health;
    public void RemoveHealth(int health) => _currentHealth -= health;
    public void AddHealth(int health) => _currentHealth += health;
    
}
