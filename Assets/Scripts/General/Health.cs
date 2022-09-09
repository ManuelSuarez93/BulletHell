using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using General;

public class Health : MonoBehaviour
{
    [SerializeField] int _maxHealth;  
    [SerializeField] HealEvents _healEvents;
    [Header("Timer")]
    [SerializeField] bool _useTimer = false;
    [SerializeField] float _timeBeforeHealAgain, _timeBeforeDamage;
    private Timer _timer;
    private bool _canDamage = true;
    [System.Serializable]
    public struct HealEvents
    {
        public UnityEvent 
            OnDamageEvent, 
            OnDeathEvent, 
            OnHealEvent;
    }
    private int _currentHealth; 
    public int CurrentHealth => _currentHealth;
    public float HealthPcnt => (float)_currentHealth/ _maxHealth;
    private void Awake()
    {
        _currentHealth = _maxHealth;  
        if(_useTimer)
            SetTimer();
    }
    private void SetTimer()
    {
        _timer = gameObject.AddComponent<Timer>();
        _timer.SetTimer(_timeBeforeDamage);
        _timer.Stop(true);

        _timer.SetFinishAction(() =>
        {
            _canDamage = true;
            _timer.Stop(true);
        });

        _timer.SetUpdateAction(() =>
        {
            PlayerEffects.Instance.Invicibility(_timer.TimePcnt);
        });
    }
    public void ChangeHealth(int health) => _currentHealth = health;
    public void RemoveHealth(int health)
    {
        if(_canDamage)
        {
            _healEvents.OnDamageEvent.Invoke(); 
            _currentHealth -= health;

            if(_useTimer)
            {
                _canDamage = false;
                _timer.RestartTimer();
                _timer.Stop(false);
            }
        }
    } 
    public void AddHealth(int health) 
    { 
        _currentHealth += health;
        _healEvents.OnHealEvent.Invoke();
    }
    public void ResetHealth() => _currentHealth = _maxHealth;
}
