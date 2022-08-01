using System.Collections.Generic;
using System;
using System.Data;
using UnityEngine;

public class PlayerProjectile: MonoBehaviour, IMovable
{ 
    [SerializeField] LayerMask _collisionLayer;
    [SerializeField] LayerMask _endOfLevelLayer;
    private float _speed;
    private Vector3 _direction;
    private int _damage;
    private PrefabPool _pool;
    private GameObject _gameObject; 

    public Vector3 Direction => _direction; 
    public float Speed => _speed; 
    public PrefabPool Pool => _pool; 
    public GameObject GameObject => _gameObject; 
    public void SetDirection(Vector3 direction) => _direction = direction; 
    public void SetPool(PrefabPool newPool) => _pool = newPool;
    public void SetDamage(int damage) => _damage = damage;
    public void SetSpeed(float speed) => _speed = speed;
     
    private void Update() => transform.Translate(_direction * Time.deltaTime * _speed);

    private void ReturnObjectToPool() => _pool.ReturnPrefab(this.gameObject, true);


    private void OnTriggerEnter(Collider other)
    {
        if ((_collisionLayer & (1 << other.gameObject.layer)) > 0  ) 
        {
            other.GetComponent<Health>()?.RemoveHealth(_damage);
            ReturnObjectToPool();
        }
        else if ((_endOfLevelLayer.value & 1 << (other.gameObject.layer)) > 0  ) 
            ReturnObjectToPool(); 
            
    }

    
}
