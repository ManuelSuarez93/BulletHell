using UnityEngine;
using Pooling;

public class Projectile: MonoBehaviour, IMovable, IPoolable
{ 
    [SerializeField] bool _destroyOnEnd = true, _isMoving = true, _follosTarget = false;
    [SerializeField] LayerMask _collisionLayer;
    [SerializeField] LayerMask _endOfLevelLayer;
    private float _speed;
    private Vector3 _direction;
    private int _damage;
    private PrefabPool _pool;
    private GameObject _gameObject;  
    private Transform _follow;
    public Vector3 Direction => _direction; 
    public float Speed => _speed; 
    public PrefabPool Pool => _pool; 
    public GameObject GameObject => _gameObject; 
    public void SetDirection(Vector3 direction) => _direction = direction; 
    public void SetPool(PrefabPool newPool) => _pool = newPool;
    public void SetDamage(int damage) => _damage = damage;
    public void SetSpeed(float speed) => _speed = speed;
     
    private void Update()
    {
        if(_isMoving)
        {
            if(_follosTarget)
                transform.position = _follow.position;
            else
                transform.Translate(_direction * Time.deltaTime * _speed);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_collisionLayer & 1 << other.gameObject.layer) > 0  ) 
        {
            other.GetComponent<Health>()?.RemoveHealth(_damage);
            if(_destroyOnEnd) ReturnObjectToPool();
        }
        else if ((_endOfLevelLayer.value & 1 << other.gameObject.layer) > 0  ) 
            if(_destroyOnEnd) ReturnObjectToPool();  
    }
    public void SetFollow(Transform newFollow) => _follow = newFollow;
    public void ReturnObjectToPool() => _pool.ReturnPrefab(this.gameObject, true);



    
}
