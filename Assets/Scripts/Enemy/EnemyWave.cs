using System.Collections.Generic;
using UnityEngine;
using static General.Global;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "BulletHell/Enemy/EnemyWave", order = 0)]
public class EnemyWave : ScriptableObject 
{
    [SerializeField] private float _waveSpeedIncrease = 1f;
    [SerializeField] private float _waveRate = 15f;
    [SerializeField] private float _wavePauseTime = 1f; 
    [SerializeField] private int _level;
    [SerializeField] private int _difficulty;
    [SerializeField] private int _levelArea;
    [SerializeField] private List<WavePartData> _enemyDatas;

    public float WaveSpeedIncrease { get => _waveSpeedIncrease; set => _waveSpeedIncrease = value; }
    public float WaveRate { get => _waveRate; set => _waveRate = value; }
    public float WavePauseTime { get => _wavePauseTime; set => _wavePauseTime = value; }
    public int Level { get => _level; set => _level = value; }
    public int Difficulty { get => _difficulty; set => _difficulty = value; }
    public List<WavePartData> EnemyDatas { get => _enemyDatas; set => _enemyDatas = value; }

   
}  
 
