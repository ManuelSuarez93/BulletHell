using General;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "BulletHell/Weapon")]
    public class WeaponData : ScriptableObject
    {
        public WeaponType WeaponType;
        public float ShootRate;
        public int Damage; 
        public float ProjectileSpeed; 
        public GameObject Projectile;
 

    }
}

 
    