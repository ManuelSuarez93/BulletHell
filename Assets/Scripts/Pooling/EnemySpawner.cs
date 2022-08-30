using static General.Global;
using System.Collections.Generic; 
using UnityEngine;
 
namespace Pooling
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<PrefabPool> _pool;
        [SerializeField] private Collider _player;
        [Header("Enemy settings")]
        [SerializeField] private Direction _direction = Direction.up;
        [SerializeField] private float _rate = 1f;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private bool _finishInObjective = false;
        [SerializeField] private Collider _objective;

        [Header("Enemy movement settings")]
        [SerializeField] private bool _animate = false;
        [SerializeField] private float _animSpeed = 1f;
        [SerializeField] private float _amplitude = 1f; 

        [Header("Spawner settings")]
        [SerializeField] private bool _spawnRandom = false;
        [SerializeField] private int _spawnAmount = 1;
        [SerializeField] private List<Transform> _spawnPoints;

        [Header("Wave settings")]
        [SerializeField] private float _waveSpeedIncrease = 1f;
        [SerializeField] private float _waveRate = 15f;
        [SerializeField] private float _wavePauseTime = 1f;
        [SerializeField] private int _spawnsPerWave = 5;

        private float _timer, _waveTimer;
        private int _currentWave;
        private bool _wavePause, _maxSpawnsReached;
        List<GameObject> _currentlySpawned = new List<GameObject>();
        Vector3 _vectorDirection =>
            _direction == Direction.left ? Vector3.left :
            _direction == Direction.right ? Vector3.right :
            _direction == Direction.up ? Vector3.up :
            _direction == Direction.down ? Vector3.down :
            _direction == Direction.forward ? Vector3.forward :
            _direction == Direction.back ? Vector3.back : Vector3.zero;
        
        private void Start()
        { 
            _currentWave = 0;
            _timer = 0f;
        }
        
        private void Update()
        { 
            if (!_maxSpawnsReached)
            { 
                if (_waveTimer <= _waveRate && !_wavePause)
                {
                    SummonPrefabTimer();
                    if (_currentlySpawned.Count == _spawnsPerWave)
                        _maxSpawnsReached = true;

                    _waveTimer += Time.deltaTime;
                }
                else if (_waveTimer >= _waveRate && !_wavePause)
                {
                    _waveTimer = 0f;
                    _timer = 0f;
                    _currentWave++;
                    _speed += _waveSpeedIncrease;
                    _wavePause = true;
                }
                else if (_wavePause)
                {
                    if (_timer <= _wavePauseTime)
                        _timer += Time.deltaTime;
                    else
                        _wavePause = false;
                }
            }
            else
            {
                if (_currentlySpawned.Count == 0)
                    _maxSpawnsReached = false;
            }
        
        }

        private void SummonPrefabTimer()
        {
            if (_timer >= _rate)
            {
                _timer = 0f;
                if (_spawnRandom)
                {
                    int num = Random.Range(1, _spawnAmount);
                    for (int i = 0; i < num; i++)
                        SpawnObject(_pool[Random.Range(0, _pool.Count)]);
                }
                else 
                    SpawnObject(_pool[Random.Range(0, _pool.Count)]);  
            }
            else 
                _timer += Time.deltaTime; 
        }
        
        private void SpawnObject(PrefabPool pool)
        {
            var x = pool.GetPrefab(true); 
            x.transform.localPosition = _spawnPoints.Count > 0 ? _spawnPoints[Random.Range(0, _spawnPoints.Count)].position : transform.position;
            var e = x.GetComponent<Enemy>();
            e.SetDirection(_vectorDirection);
            e.SetPool(pool);
            e.SetSpeed(_speed);
            e.SetOnDisable(() => RemoveFromList(e.gameObject));
            e.SetObjective(_objective, _player); 
            _currentlySpawned.Add(e.gameObject);
        }

        private void RemoveFromList(GameObject o) => _currentlySpawned.Remove(o);
    }
 
}
