namespace General
{
    public enum Direction { left, right, up, down, forward, back }  
    public enum SpecialType { Shotgun, Laser, Explosion} 
    public enum WeaponType { Pistol, Rifle, MachineGun, Rocket } 

    
    [System.Serializable] 
    public struct WeaponObject
    {
        public string Name;
        public WeaponType Type;
    }
    
    [System.Serializable] 
    public struct SpecialObject
    {
        public string Name;
        public SpecialType Type;
    }
}
