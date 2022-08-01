using System.Collections.Generic; 
using UnityEngine;
 
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Direction _direction = Direction.up;
    [SerializeField] private float _rate;
    [SerializeField] private float _speed;
    [SerializeField] private bool _spawnRandom;
    [SerializeField] private int _spawnAmount = 1;
    [SerializeField] private PrefabPool _pool;
    [SerializeField] private List<Transform> _spawnPoints; 
    [SerializeField] private float _waveSpeedIncrease;
    [SerializeField] private float _waveRate;
    [SerializeField] private float _wavePauseTime;

    private float _timer, _waveTimer;
    private int _currentWave;
    private bool _wavePause;
    List<GameObject> _currentlySpawned;
    Vector3 _vectorDirection =>
        _direction == Direction.left ? Vector3.left :
        _direction == Direction.right ? Vector3.right :
        _direction == Direction.up ? Vector3.up :
        _direction == Direction.down ? Vector3.down :
        _direction == Direction.forward ? Vector3.forward :
        _direction == Direction.back ? Vector3.back : Vector3.zero;
    enum Direction { left, right, up, down, forward, back }
    private void Start()
    {
        _currentWave = 0;
        _timer = 0f;
    }
     
    private void Update()
    { 
        if (_waveTimer <= _waveRate && !_wavePause)
        {
            SummonPrefabTimer();
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

    private void SummonPrefabTimer()
    {
        if (_timer >= _rate)
        {
            _timer = 0f;
            if(_spawnRandom)
            { 
                int num = Random.Range(1, _spawnAmount);
                for (int i = 0; i < num; i++)
                    SpawnObject();
            }
            else 
                SpawnObject();  
        }
        else 
            _timer += Time.deltaTime; 
    }

    private void SpawnObject()
    {
        var x = _pool.GetPrefab(true);
        x.transform.parent = null;
        x.transform.localPosition = _spawnPoints.Count > 0 ? _spawnPoints[Random.Range(0, _spawnPoints.Count)].position : transform.position;
        var e = x.GetComponent<IMovable>();
        e.SetDirection(_vectorDirection);
        e.SetPool(_pool);
        e.SetSpeed(_speed);
    }
}
 