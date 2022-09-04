using System; 
using UnityEngine;
using General; 
using static General.Global;
using Pooling;
public class EnemyBehaviour : MonoBehaviour, IMovable, IPoolable
{
    #region Fields
    [SerializeField] private Health _health;
    [SerializeField] private LayerMask _collisionLayer, _endOfLevelLayer; 

    [Header("Moving settings")]
    [SerializeField] private bool _animation = false;
    [SerializeField] private float _animationSpeed = 1f;
    [SerializeField] private float _amplitude;
    [SerializeField] private float _animationRate = 0.5f;
    [SerializeField] private bool _finishInObjective;
    [SerializeField] private Direction _direction;
    [SerializeField] private bool _finishInPlayer = false;
    [Header("Shooting Settings")] 
    [SerializeField] private bool _shootPlayer = false;
    [SerializeField] private PrefabPool _projectiles;
    [SerializeField] private float _shootRate;
    [SerializeField] private float _shootSpeed;
    [SerializeField] private Transform _shootPosition;
    [Header("Timer")]
    [SerializeField] bool _useTimer;
    [SerializeField] private Timer _timer;

    private int _damage;
    private float _speed;
    private PrefabPool _pool; 
    private Action _onDisable;
    private Collider _girdObj;   
    #endregion

    #region Setters
    public void SetDirection(Direction newDirection) => _direction = newDirection;
    public void SetSpeed(float speed) => _speed = speed;
    public void SetDamage(int damage) => _damage = damage;
    public void SetPool(PrefabPool newPool) => _pool = newPool;
    public void SetObjective(Collider objective) => _girdObj = objective;
    public void SetAnimation(bool animate, float amplitude = 0f, float animationSpeed = 0f)
    {
        _animation = animate;
        _amplitude = amplitude;
        _animationSpeed = animationSpeed;
    }
    #endregion
    private void ReturnToPool() => _pool.ReturnPrefab(this.gameObject, true);

    private void Start()
    {
        if(_useTimer)
        {
            _timer.SetFinishAction(() =>
                {
                    _timer.Stop(true);
                    Shoot();
                    _timer.Stop(false);
                });

            _timer.SetTimer(_shootRate);
        }
            
    }
    private void Update() => Movement();

    private void Movement()
    { 
        if(_finishInObjective)
        { 
            if (_girdObj != null && Vector3.Distance(_girdObj.transform.position, transform.position) > 0) 
                transform.position = Vector3.MoveTowards(transform.position, 
                                    new Vector3(_girdObj.transform.position.x, 
                                                transform.position.y,
                                                 _girdObj.transform.position.z),
                                    Time.deltaTime * _speed); 
        } 
        else
        {
            if (_finishInPlayer && GameManager.Instance.PlayerObject != null) 
                transform.position = Vector3.Lerp(transform.position, GameManager.Instance.PlayerTransform.position, Time.deltaTime * _speed); 
            else 
                transform.Translate(VectorDirection(_direction) * Time.deltaTime * _speed); 
        }

         if (_animation) 
            transform.position = Wave(transform.position.y, transform.position.z, Time.time * _animationSpeed);  
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
        if(_shootPlayer)
            e.SetFollow(GameManager.Instance.PlayerTransform);
        else
            e.SetDirection( _direction);
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
