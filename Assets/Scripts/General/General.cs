using UnityEngine; 
using System.Collections.Generic;
using System.Linq;

namespace General
{   
    public static class Global
    {
        public enum Direction { left, right, up, down, forward, back, upleft, upright, downright, downleft}  
        public enum EnemyType{Stay, Moving}
        public enum SpecialType { Shotgun, Laser, Explosion} 
        public enum WeaponType { Pistol, Rifle, MachineGun, Rocket }
        public const string SPECIAL_FOLDER = "Specials";
        public const string WEAPON_FOLDER = "Weapons";
        public const string ENEMYWAVE_FOLDER = "EnemyWaves";
        public const string ENEMY_FOLDER = "Enemy"; 
        public const string LEVEL_FOLDER = "Levels";
        public static List<T> LoadResources<T>(string folderName) where T : Object => Resources.LoadAll<T>(folderName).ToList();
        public  static Vector3 VectorDirection(Direction direction)
        {
            switch (direction)
            {
                
                case Direction.left : return Vector3.left;
                case Direction.right : return Vector3.right;
                case Direction.back : return Vector3.back;
                case Direction.forward : return Vector3.forward;
                case Direction.down : return Vector3.down;
                case Direction.up : return Vector3.up;
                case Direction.upleft : return (Vector3.forward - Vector3.left).normalized;
                case Direction.upright : return (Vector3.forward - Vector3.right ).normalized;
                case Direction.downleft: return (Vector3.down - Vector3.left).normalized;
                case Direction.downright : return (Vector3.down - Vector3.left).normalized;


                default: return Vector3.zero;
            } 
        } 
        [System.Serializable]
    public struct EnemyTypeData
        {
            public EnemyType Type;
            public int SpawnAmount;
            public float TimeAfterNextSpawn;
            public float TimeAfterNextWave;
        }
        [System.Serializable]
        public struct Ship
        {
            public string Name;
            public SpecialType Special;
            public WeaponType Weapon;
            public GameObject ShipObject; 
        }
    }
    
}
