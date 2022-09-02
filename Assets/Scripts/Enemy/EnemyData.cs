using UnityEngine;
using static General.Global;

[CreateAssetMenu(fileName = "EnemyData", menuName = "BulletHell/Enemy/EnemyData", order = 0)] 
public class EnemyData : ScriptableObject 
{
    [SerializeField] private GameObject _enemyObject;
    [SerializeField] private EnemyType _type;

    public EnemyType Type => _type;
    public GameObject EnemyObject => _enemyObject;
}