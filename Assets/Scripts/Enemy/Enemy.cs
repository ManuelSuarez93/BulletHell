using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour, IMovable
{
    [SerializeField] private Health _health;
    [SerializeField] LayerMask _collisionLayer;
    [SerializeField] LayerMask _endOfLevelLayer;
    [SerializeField] Collider _objective;
    private int _damage;
    private Vector3 _direction = Vector3.down;
    private float _speed;
    private PrefabPool _pool;
    private GameObject _go;
    private Action _onDisable;

    public PrefabPool Pool => _pool;
    public GameObject GameObject => _go; 
    public Vector3 Direction => _direction; 
    public float Speed => _speed; 
    public void SetDirection(Vector3 direction) => _direction = direction;
    public void SetSpeed(float speed) => _speed = speed;
    public void SetDamage(int damage) => _damage = damage;
    public void SetPool(PrefabPool newPool) => _pool = newPool;
    public void SetObjective(Collider objective) => _objective = objective; 
    private void ReturnToPool() => _pool.ReturnPrefab(this.gameObject, true);

    void Update()
    {
        if (transform.position.z <=  _objective.bounds.center.z) return;

        transform.Translate(_direction * Time.deltaTime * _speed);
    }

    public void CheckIfDeath()
    {
        if (_health.CurrentHealth <= 0) ReturnToPool();
    }
    public void SetOnDisable(Action action) => _onDisable = action; 
    private void OnDisable() => _onDisable?.Invoke();
    private void AddToScore() => UIManager.Instance.AddToScore(1);

    private void OnTriggerEnter(Collider other)
    {
        if ((_collisionLayer & 1 << other.gameObject.layer) > 0)
        {
            other.GetComponent<Health>()?.RemoveHealth(_damage);
            AddToScore();
            CheckIfDeath();
        }
        else if ((_endOfLevelLayer.value & 1 << other.gameObject.layer) > 0)
            ReturnToPool();
    }
}
