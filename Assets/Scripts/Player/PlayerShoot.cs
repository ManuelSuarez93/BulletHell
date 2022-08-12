using System.Collections.Generic; 
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private List<Special> _special;
    [SerializeField] private Direction _direction;
    private Weapon _currentWeapon;
    private Special _currentSpecial;
    private int _currentWeaponIndex;
    private float _timer;

    private Vector3 _vectorDirection =>
     _direction == Direction.left ? Vector3.left :
     _direction == Direction.right ? Vector3.right :
     _direction == Direction.up ? Vector3.up :
     _direction == Direction.down ? Vector3.down :
     _direction == Direction.forward ? Vector3.forward :
     _direction == Direction.back ? Vector3.back : Vector3.zero;

    enum Direction { left, right, up, down, forward, back }

    [System.Serializable]
    internal class Weapon
    {
        [SerializeField] private string _name;
        [SerializeField] private float _shootRate;
        [SerializeField] private float _speed = 1f;
        [SerializeField] private int _damage;
        [SerializeField] private int _projectilesAmount;
        [SerializeField] private GameObject _projectile;
        [SerializeField] private WeaponType _type; 
        [SerializeField] private PrefabPool _pool;
        public enum WeaponType { Pistol, Rifle, MachineGun, Rocket }
        public string Name => _name;
        public float ShootRate => _shootRate;
        public float Speed => _speed;
        public int Damage => _damage; 
        public GameObject Projectile => _projectile;
        public WeaponType Type => _type;
        public PrefabPool Pool => _pool;
    }

    [System.Serializable]
    internal class Special
    {
        [SerializeField] private string _name; 
        [SerializeField] private float _speed = 1f;
        [SerializeField] private int _damage;
        [SerializeField] private int _projectilesAmount;
        [SerializeField] private GameObject _projectile;
        [SerializeField] private SpecialType _type;
        [SerializeField] private PrefabPool _pool;

        public enum SpecialType { Shotgun, Laser, Explosion}
        public string Name => _name; 
        public float Speed => _speed;
        public int Damage => _damage;
        public int ProjectilesAmount => _projectilesAmount;
        public GameObject Projectile => _projectile;
        public SpecialType Type => _type;
        public PrefabPool Pool => _pool;
    }
    private void Start()
    {
        _timer = 0f;
        _currentWeapon = _weapons?[0];
        _currentSpecial = _special?[0];
        _currentWeaponIndex = 0;
        UIManager.Instance.SetWeaponName(_currentWeapon.Name);
    }

    public void Update()
    {
        if (_timer < _currentWeapon.ShootRate)
            _timer += Time.deltaTime;
        else
        {
            _timer = 0f;
            Shoot();
        }
    }

    private void Shoot()
    {
        var x = _currentWeapon.Pool.GetPrefab(true);
        x.transform.localPosition = transform.localPosition; 
        var proj = x.GetComponent<Projectile>();
        proj.SetDirection(_vectorDirection);
        proj.SetPool(_currentWeapon.Pool);
        proj.SetSpeed(_currentWeapon.Speed);
        proj.SetDamage(_currentWeapon.Damage);
    }

    private void Shotgun()
    {
        var angleDifference = _currentSpecial.ProjectilesAmount > 1 ? (180f / (_currentSpecial.ProjectilesAmount - 1)) : 0;
        var totalAngle = -90f;
        for (int i = 0; i < _currentSpecial.ProjectilesAmount; i++)
        {
            var x = _currentSpecial.Pool.GetPrefab(true);
            x.transform.localPosition = transform.localPosition;
            if (_currentSpecial.ProjectilesAmount > 1)
            {
                x.transform.localEulerAngles = new Vector3(0, totalAngle, 0);
                totalAngle += angleDifference;
            }
            var proj = x.GetComponent<Projectile>();
            proj.SetDirection(_vectorDirection);
            proj.SetPool(_currentSpecial.Pool);
            proj.SetSpeed(_currentWeapon.Speed);
            proj.SetDamage(_currentWeapon.Damage);
        }
    }

    public void SpecialWeapon()
    { 
        switch(_currentSpecial.Type)
        {
            case Special.SpecialType.Shotgun: Shotgun(); break;
            case Special.SpecialType.Explosion: Shotgun(); break;
            case Special.SpecialType.Laser: Shotgun(); break;
        }
    }

    public void SwitchWeapon()
    {
        _currentWeaponIndex = _currentWeaponIndex == _weapons.Count - 1 ? 0 : ++_currentWeaponIndex;
        _currentWeapon = _weapons[_currentWeaponIndex]; 
        UIManager.Instance.SetWeaponName(_currentWeapon.Name);
    }
}
