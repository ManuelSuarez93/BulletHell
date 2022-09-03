using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "BulletHell/Level", order = 0)]
public class Level : ScriptableObject 
{
    public List<EnemyWave> Waves;
    public int Difficulty;
}
