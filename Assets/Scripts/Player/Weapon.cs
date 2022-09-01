using System.Collections.Generic;
using System.Linq;
using static General.Global;
using General;
using UnityEngine;
using Pooling;

namespace Player
{
    [System.Serializable]
    public class Weapon
    {   
        [SerializeField] private PrefabPool _pool;
        [SerializeField] private GameObject _supportProjectile;
        [SerializeField] private bool _isSupportWeapon;
        [SerializeField] private Direction _direction;
        [SerializeField] private Timer _timer;

        private List<WeaponData> _weaponData;  
        private WeaponData _currentData;
        private bool _canShoot;
        public float ShootRate => _currentData.ShootRate; 

        public void InitializeWeapon(Ship newShip) 
        {        
            _weaponData = 
                Resources
                .LoadAll<WeaponData>("Weapons")
                .ToList();
             
            _currentData = 
                    _isSupportWeapon ? _weaponData?.FirstOrDefault(x => x.WeaponType == WeaponType.Pistol) :  
                    _weaponData?.FirstOrDefault(x => x.WeaponType == newShip.Weapon);  


            _timer.SetFinishAction(() => 
                {
                    _canShoot = true;
                    _timer.Stop(true);
                });

            _timer.SetTimer(_currentData.ShootRate);
            _timer.Stop(false);
        }
                
        public void Shoot(Transform transform, Vector3 vectorDirection = default(Vector3))
        {
            if(!_canShoot) return;

                var x = _pool.GetPrefab(true);
            x.transform.position = transform.position; 
            var proj = x.GetComponent<Projectile>();
            proj.SetDirection(_isSupportWeapon? VectorDirection(_direction) :  vectorDirection);
            proj.SetPool(_pool);
            proj.SetSpeed(_currentData.ProjectileSpeed);
            proj.SetDamage(_currentData.Damage);

            _canShoot = false;
            _timer.Stop(false);
        } 
 
        public void SetWeapon(WeaponType type)
        {
            var data = _weaponData.FirstOrDefault(x => x.WeaponType == type);

            if(data!= null) 
                _currentData = data; 
            else 
                Debug.LogError($"Error: no weapon data found for type:{type}"); 

            _timer.SetTimer(_currentData.ShootRate);
            
        }



    } 
}

 
    