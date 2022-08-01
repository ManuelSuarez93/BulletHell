using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons;
    private Weapon _currentWeapon;
    private int _currentWeaponIndex;

    [System.Serializable]
    public class Weapon
    {
        [SerializeField] string _name;
        [SerializeField] float _shootRate;
        [SerializeField] int _damage;
        [SerializeField] int _projectilesAmount;
        [SerializeField] GameObject _projectile;

        public string Name => _name;
        public float ShootRate => _shootRate;
        public int Damage => _damage;
        public int ProjectilesAmount => _projectilesAmount;
        public GameObject Projectile => _projectile;
    } 
    private void Start()
    {
        _currentWeapon = _weapons?[0];
        _currentWeaponIndex = 0;
    }

    public void Update()
    {

    }
    public void Switch(bool next)
    {
        _currentWeaponIndex =
            next && _currentWeaponIndex == _weapons.Count ? _currentWeaponIndex = 0 :
            next ? _currentWeaponIndex + 1 :
            !next && _currentWeaponIndex == 0 ? _currentWeaponIndex = _weapons.Count :
            !next ? _currentWeaponIndex - 1 : _currentWeaponIndex;

        _currentWeapon = _weapons[_currentWeaponIndex];
    }
}
