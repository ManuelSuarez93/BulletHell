using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{ 
    [SerializeField] private int _maxHealth = 5;
    [SerializeField] private LayerMask _damageLayer;
    [SerializeField] Damagable damagable;

    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        damagable.SetAction(() => Damage());
        UIManager.Instance.SetHealth(_currentHealth);
    }
     
    private void Damage()
    {
        _currentHealth--;
        UIManager.Instance.SetHealth(_currentHealth);
    }
}
