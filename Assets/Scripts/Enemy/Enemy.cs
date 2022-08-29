using System; 
using UnityEngine; 
using General;
using Pooling;
public class Enemy : MonoBehaviour, IMovable, IPoolable
{
    [SerializeField] private Health _health;
    [SerializeField] LayerMask _collisionLayer;
    [SerializeField] LayerMask _endOfLevelLayer;
    [SerializeField] Collider _objective;
    [SerializeField] Collider _player;
    [SerializeField] private float _animationSpeed = 1f;
    [SerializeField] private float _amplitude;
    [SerializeField] private float _animationRate = 0.5f;
    [SerializeField] private bool _animation = false;
    [SerializeField] private bool _finishInObjective;
    [SerializeField] private bool _finishInPlayer = false;
    [Header("Shooting Settings")]
    [SerializeField] private bool _enemyShoot = false;
    [SerializeField] private bool _shootAtPlayer = false;
    [SerializeField] private PrefabPool _projectiles;
    [SerializeField] private float _shootRate;
    [SerializeField] private float _shootSpeed;
    [SerializeField] private Transform _shootPosition;

    private int _damage;
    private Vector3 _direction = Vector3.down;
    private float _speed, _timer = 0f;
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
    public void SetObjective(Collider objective, Collider player)
    {
        _objective = objective;
        _player = player;
    }
    public void SetAnimation(bool animate, float amplitude = 0f, float animationSpeed = 0f)
    {
        _animation = animate;
        _amplitude = amplitude;
        _animationSpeed = animationSpeed;
    }
    private void ReturnToPool() => _pool.ReturnPrefab(this.gameObject, true);

    private void  Update()
    {
        Movement();

        if (_enemyShoot)
        {
            if (_timer <= _shootRate)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                _timer = 0f;
                Shoot();
            }
        }

        if (_animation) transform.position = Wave(transform.position.y, transform.position.z, Time.time * _animationSpeed);


    }

    private void Movement()
    { 
        if(_finishInObjective)
        { 
            if (_objective != null && transform.localPosition.z >= _objective.bounds.center.z) 
                transform.Translate(_direction * Time.deltaTime * _speed); 
        } 
        else
        {
            if (_finishInPlayer && _player != null) 
                transform.position = Vector3.Lerp(transform.position, _player.transform.position, Time.deltaTime * _speed); 
            else 
                transform.Translate(_direction * Time.deltaTime * _speed); 
        }
             
      
    }

    public void CheckIfDeath()
    {
        if (_health.CurrentHealth <= 0)
        {
            ReturnToPool();
            _health.ResetHealth();
        }
    }
    public void SetOnDisable(Action action) => _onDisable = action; 
    private void OnDisable() => _onDisable?.Invoke();
    private void AddToScore() => GameManager.Instance.SetScore(GameManager.Instance.Score + 1);

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
     
    private void Shoot()
    {
        var x = _projectiles.GetPrefab(true);
        x.transform.localPosition = _shootPosition.localPosition;
        var e = x.GetComponent<Projectile>();
        e.SetDirection(_shootAtPlayer ? new Vector3(_player.transform.position.x,0 , _player.transform.position.z) : _direction);
        e.SetPool(_projectiles);
        e.SetSpeed(_speed + _shootSpeed);  
    }

    public Vector3 Wave(float u, float v, float t)
    {
        Vector3 p;
        p.y = u;
        p.z = v;
        p.x = Mathf.Sin(Mathf.PI * (t + ((u + v) * _animationRate))) * _amplitude;

        return p;
    }
}
