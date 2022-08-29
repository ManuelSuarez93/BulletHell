using System;
using System.Collections.Generic;
using General;
using UnityEngine;

namespace Player
{
    public class PlayerShoot : MonoBehaviour
    {
        #region  Fields
        [SerializeField] private List<WeaponObject> _weapons;
        [SerializeField] private List<SpecialObject> _specials;
        [SerializeField] private Direction _direction; 
        [SerializeField] private Weapon _weapon; 
        [SerializeField] private Special _special;

        private WeaponType _currentWeaponType;
        private int _currentWeaponIndex;

        private SpecialType _currentSpecialType;
        private int _currentSpecailIndex;
        private float _timer; 
        
        private Vector3 _vectorDirection =>
                        _direction == Direction.left ? Vector3.left :
                        _direction == Direction.right ? Vector3.right :
                        _direction == Direction.up ? Vector3.up :
                        _direction == Direction.down ? Vector3.down :
                        _direction == Direction.forward ? Vector3.forward :
                        _direction == Direction.back ? Vector3.back : Vector3.zero;
 
        #endregion
        #region  Structs

        
        #endregion
        #region Unity Methods
        void Awake()
        {    
            _weapon.InitializeWeapon(); 
            _weapon.SetWeapon((WeaponType)0);
            _special.InitializeSpecial();
        }

        private void Start()
        {
            _timer = 0f;
            _currentWeaponType = _weapons[0].Type;
            _currentSpecialType = _specials[0].Type;
            _currentWeaponIndex = 0; ;
        }

        public void Update()
        {
            _weapon.Shoot(transform, _vectorDirection); 
        }
        #endregion
        
        public void UseSpecial() => _special.UseSpecialcial(transform, _vectorDirection, _currentSpecialType);

        public void SwitchWeapon()
        {
            _currentWeaponIndex = _currentWeaponIndex == _weapons.Count - 1 ? 0 : ++_currentWeaponIndex;
            _currentWeaponType = _weapons[_currentWeaponIndex].Type;  
        }
 
    }
    
}
 