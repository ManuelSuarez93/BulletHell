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
        [Header("Enemy settings")]  
        [SerializeField] private float _speed = 5f; 

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
        private List<Level> _levels;
        private GridPoint _gridPoint;
        private Vector2Int _gridPointPos;
        private List<EnemyData> _enemies; 
        private WavePartData _currentData;
        private EnemyWave _currentWave;
        private bool _isCurrentWaveFinished;
        private bool _isWavePartEnded;
        private int _currentDataIndex, _currentSpawnedWavePart;
        private Dictionary<EnemyType, PrefabPool> _poolDictionary = new Dictionary<EnemyType, PrefabPool>();
        List<GameObject> _currentlySpawned = new List<GameObject>(); 

        void Awake()
        {      
            _currentDataIndex = -1;
            _enemies = LoadResources<EnemyData>(ENEMY_FOLDER);
            _levels = LoadResources<Level>(LEVEL_FOLDER);
            
            for (int i = 0; i < _pools.Count; i++)
            { 
                EnemyType type = (EnemyType)System.Enum.GetValues(typeof(EnemyType)).GetValue(i);
                _pools[i].gameObject.name = type.ToString();
                _pools[i].SetPrefab(_enemies.FirstOrDefault(x => x.Type == type)?.EnemyObject);
                _pools[i].CreatePrefabs();
                _poolDictionary.Add(type,_pools[i]); 
            }
 
            SetWave();
            SetCurrentEnemy();
        }  
        

        private void Update()
        {
            if(_currentSpawnedWavePart == _currentData.SpawnAmount)
                SetCurrentEnemy();
        }
        private void SetWave()
        { 
            _currentWave = _levels?[0].Waves[0]; 
            _waveTimer.SetTimer(_currentWave.WaveRate);
            _waveTimer.RestartTimer();
            _waveTimer.Stop(false);
            _isCurrentWaveFinished = false; 
        }
        
        private void SetCurrentEnemy()
        {  
            if(_currentDataIndex < _currentWave.EnemyDatas.Count - 1) 
                _currentDataIndex++;
            else 
                _currentDataIndex = 0;  

            _currentSpawnedWavePart = 0;
            _currentData = _levels[0].Waves[0].EnemyDatas[_currentDataIndex]; 
            _summonTimer.SetFinishAction(() =>
            {
                _summonTimer.Stop(true);
                SpawnObject(_poolDictionary[_currentData.Type]);
            });

            _summonTimer.SetTimer(_currentWave.EnemyDatas[0].TimeAfterNextSpawn);
            _gridPointPos = new Vector2Int(_currentData.StartingGridX, _currentData.StartingGridY);
            _gridPoint = _grid.GetGridPoint(_gridPointPos); 
        }

        private void SpawnObject(PrefabPool pool)
        {
            var x = pool.GetPrefab(true); 
            x.transform.localPosition = _spawnPoints.Count > 0 ? _spawnPoints[Random.Range(0, _spawnPoints.Count)].position : transform.position;
            var e = x.GetComponent<EnemyBehaviour>();
            e.SetPool(pool);
            e.SetSpeed(_speed);
            e.SetOnDisable(() => RemoveFromList(e.gameObject));

            SetCurrentGridPoint();
            e.SetObjective(_gridPoint.Collider.transform); 


            _currentlySpawned.Add(e.gameObject);
            _currentSpawnedWavePart++;

            _summonTimer.Stop(false);
            
        }
        public void SetCurrentGridPoint()
        { 
            if(GoDown)
            {
                _gridPointPos.y -= _currentData.GridSeparationY; 
                if(_gridPointPos.y >= _currentData.EndGridY)
                {
                    _gridPointPos.x +=_currentData.GridSeparationX; 
                    if(_gridPointPos.x > _currentData.EndGridX)  
                    { 
                        _gridPointPos.x = _currentData.StartingGridX;   
                    }
                } 
                else
                {
                    _gridPointPos.y = _currentData.StartingGridY;
                }
            }
            else
            {
                _gridPointPos.y += _currentData.GridSeparationY;  
                if(_gridPointPos.y <= _currentData.EndGridY)
                {
                    _gridPointPos.x +=_currentData.GridSeparationX; 
                    if(_gridPointPos.x > _currentData.EndGridX)  
                    {
                        _gridPointPos.x = _currentData.StartingGridX;   
                    }
                }
            }
           

            _gridPoint = _grid.GetGridPoint(_gridPointPos);
          
        }
        private void RemoveFromList(GameObject o) => _currentlySpawned.Remove(o);
        public bool GoDown => _currentData.StartingGridY > _currentData.EndGridY;
    }
 
}
