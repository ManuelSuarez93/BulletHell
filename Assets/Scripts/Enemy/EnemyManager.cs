using static General.Global;
using System.Collections.Generic;
using UnityEngine;
using Pooling; 
using System.Linq;
using General;

namespace Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private List<PrefabPool> _pools;
        [SerializeField] private Timer _waveTimer, _summonTimer;
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
        [Header("Grid")]
        [SerializeField] private GridMap _grid;
        private List<EnemyData> _enemies; 
        private List<EnemyWave> _waveData;
        private EnemyData _currentData;
        private EnemyWave _currentWave;
        private Dictionary<EnemyType, PrefabPool> _poolDictionary = new Dictionary<EnemyType, PrefabPool>();
        List<GameObject> _currentlySpawned = new List<GameObject>(); 

        void Awake()
        {     
            _waveData = LoadResources<EnemyWave>(ENEMYWAVE_FOLDER);
            _enemies = LoadResources<EnemyData>(ENEMY_FOLDER);

            _currentWave = _waveData?[0];
            
            for (int i = 0; i < _pools.Count; i++)
            { 
                EnemyType type = (EnemyType)System.Enum.GetValues(typeof(EnemyType)).GetValue(i);
                _pools[i].gameObject.name = type.ToString();
                _pools[i].SetPrefab(_enemies.FirstOrDefault(x => x.Type == type)?.EnemyObject);
                _pools[i].CreatePrefabs();
                _poolDictionary.Add(type,_pools[i]); 
            }

            _summonTimer.SetFinishAction(()=>
            {
                SpawnObject(_poolDictionary[_currentData.Type]);
                _summonTimer.Stop(true);
            });

            _summonTimer.SetTimer(_currentWave.WaveRate);
 
        }  
        
        private void SpawnObject(PrefabPool pool)
        {
            var x = pool.GetPrefab(true); 
            x.transform.localPosition = _spawnPoints.Count > 0 ? _spawnPoints[Random.Range(0, _spawnPoints.Count)].position : transform.position;
            var e = x.GetComponent<EnemyBehaviour>();
            e.SetDirection(VectorDirection(_direction));
            e.SetPool(pool);
            e.SetSpeed(_speed);
            e.SetOnDisable(() => RemoveFromList(e.gameObject));
            e.SetObjective(_objective, _player); 
            _currentlySpawned.Add(e.gameObject);

            _summonTimer.Stop(false);
        }

        private void RemoveFromList(GameObject o) => _currentlySpawned.Remove(o);
    }
 
}
