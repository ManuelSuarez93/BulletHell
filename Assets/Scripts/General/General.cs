using UnityEngine;

namespace General
{
    public enum Direction { left, right, up, down, forward, back }  
    public enum SpecialType { Shotgun, Laser, Explosion} 
    public enum WeaponType { Pistol, Rifle, MachineGun, Rocket } 
 
    [System.Serializable]
    public struct Ship
    {
        public string Name;
        public SpecialType Special;
        public WeaponType Weapon;
        public GameObject ShipObject; 
    }
}
