using General;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "BulletHell/Special")]
    public class SpecialData : ScriptableObject
    {
        public SpecialType Type;
        public float CooldownTime;
        public int Damage; 
        public int ProjectileAmount;
        public float ProjectileSpeed; 
        public GameObject Projectile;
        

    } 
}

 
    