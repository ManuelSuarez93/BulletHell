using System.Collections.Generic; 
using UnityEngine; 

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private PrefabPool _pool;
    [SerializeField] private Direction _direction;
    private Weapon _currentWeapon;
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
    public class Weapon
    {
        [SerializeField] string _name;
        [SerializeField] float _shootRate;
        [SerializeField] float _speed = 1f;
        [SerializeField] int _damage;
        [SerializeField] int _projectilesAmount;
        [SerializeField] GameObject _projectile;

        public string Name => _name;
        public float ShootRate => _shootRate;
        public float Speed => _speed;
        public int Damage => _damage;
        public int ProjectilesAmount => _projectilesAmount;
        public GameObject Projectile => _projectile;
    } 
    private void Start()
    {
        _timer = 0f;
        _currentWeapon = _weapons?[0];
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
        var angleDifference = _currentWeapon.ProjectilesAmount > 1 ?  (180f / (_currentWeapon.ProjectilesAmount - 1)) : 0;
        var totalAngle = -90f;
        for(int i = 0; i < _currentWeapon.ProjectilesAmount; i++)
        { 
            var x = _pool.GetPrefab(true);
            x.transform.localPosition = transform.localPosition;
            if(_currentWeapon.ProjectilesAmount > 1)
            { 
                x.transform.localEulerAngles = new Vector3(0, totalAngle, 0);
                totalAngle += angleDifference; 
            }
            var proj = x.GetComponent<Projectile>();
            proj.SetDirection(_vectorDirection);
            proj.SetPool(_pool);
            proj.SetSpeed(_currentWeapon.Speed);
            proj.SetDamage(_currentWeapon.Damage);
        }
    }

    public void Switch(bool next)
    {
        _currentWeaponIndex =
            next && _currentWeaponIndex == _weapons.Count - 1 ? _currentWeaponIndex = 0 :
            next ? _currentWeaponIndex + 1 :
            !next && _currentWeaponIndex == 0 ? _currentWeaponIndex = _weapons.Count - 1 :
            !next ? _currentWeaponIndex - 1 : _currentWeaponIndex;

        _currentWeapon = _weapons[_currentWeaponIndex];
        UIManager.Instance.SetWeaponName(_currentWeapon.Name);
    }
}
