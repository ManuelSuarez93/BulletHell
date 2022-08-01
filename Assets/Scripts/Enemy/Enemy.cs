using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerProjectile: MonoBehaviour, IMovable
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private PrefabPool _pool;
    private GameObject _gameObject;

    public Vector3 Direction => _direction;

    public float Speed => _speed;

    public PrefabPool Pool => _pool;

    public GameObject GameObject => _gameObject;

    public void SetDirection(Vector3 direction) => _direction = direction;

    public void SetPool(PrefabPool newPool) => _pool = newPool;

    public void SetSpeed(float speed) => _speed = speed;
}
public class Enemy : MonoBehaviour, IMovable
{
    [SerializeField] private Damagable _damageable; 
    [SerializeField] private int _maxHealth = 1 ; 
    private int _currentHealth;
    private Vector3 _direction = Vector3.down;
    private float _speed;
    private PrefabPool _pool;
    public GameObject _go;

    public PrefabPool Pool => _pool;
    public GameObject GameObject => _go; 
    public Vector3 Direction => _direction; 
    public float Speed => _speed; 
    public void SetDirection(Vector3 direction) => _direction = direction;
    public void SetSpeed(float speed) => _speed = speed; 
    public void SetPool(PrefabPool newPool) => _pool = newPool;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _damageable.SetAction(() => Damage());
    }

    void Update()
    {
        transform.Translate(_direction * Time.deltaTime * _speed);
    } 
    private void Damage()
    {
        _currentHealth--;
        if (_currentHealth <= 0)
        {
            gameObject.SetActive(false);
            Debug.Log($"This object: <color=cyan>{gameObject.name}</color> is dead"); 
              
            _pool.ReturnPrefab(this.gameObject, true);
        }
    }

    public void SetDirection(System.Numerics.Vector3 direction)
    {
        throw new System.NotImplementedException();
    }
}
