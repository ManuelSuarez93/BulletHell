using System.Collections.Generic;
using System.Linq;
using static General.Global;
using General;
using UnityEngine;
using Pooling;
using System;

namespace Player
{
    [System.Serializable]
    public class Weapon
    {    
        [SerializeField] private GameObject _supportProjectile;
        [SerializeField] private bool _isSupportWeapon;
        [SerializeField] private Direction _direction;
        [SerializeField] private Timer _timer; 
        [SerializeField] private List<PrefabPool> _pools; 
        private Dictionary<WeaponType, PrefabPool> _poolsDic;
        private List<WeaponData> _weaponData;  
        private WeaponData _currentData;
        private bool _canShoot;
        public float ShootRate => _currentData.ShootRate; 

        public void InitializeWeapon(Ship newShip) 
        {        
            _weaponData = LoadResources<WeaponData>(WEAPON_FOLDER);
             
            _currentData = 
                    _isSupportWeapon ? _weaponData?.FirstOrDefault(x => x.WeaponType == WeaponType.Pistol) :  
                    _weaponData?.FirstOrDefault(x => x.WeaponType == newShip.Weapon);  
                     
            //Set Timers
            _timer.SetFinishAction(() => 
                {
                    _canShoot = true;
                    _timer.Stop(true);
                });

            _timer.SetTimer(_currentData.ShootRate);
            _timer.Stop(false);

            //Set prefab pools

            if(_supportProjectile) return;

            _poolsDic = new Dictionary<WeaponType, PrefabPool>(); 

            if( _pools.Count  < Enum.GetValues(typeof(WeaponType)).Length)
                 Debug.LogError("Not enough pools for each special type");
            else
            { 
                for (int i = 0; i < _pools.Count; i++)
                { 
                    WeaponType type = (WeaponType)Enum.GetValues(typeof(WeaponType)).GetValue(i);
                    _pools[i].gameObject.name = type.ToString();
                    _pools[i].SetPrefab(_weaponData.FirstOrDefault(x => x.WeaponType == type).Projectile);
                    _pools[i].CreatePrefabs();
                    _poolsDic.Add(type,_pools[i]);
                    
                }
            } 
        }
                
        public void Shoot(Transform transform, Direction direction = default(Direction))
        {
            if(!_canShoot) return;

            var x = _isSupportWeapon ? _pools[0].GetPrefab(true) : _poolsDic[_currentData.WeaponType].GetPrefab(true);

            x.transform.position = transform.position; 
            var proj = x.GetComponent<Projectile>();
            proj.SetDirection(_isSupportWeapon ? _direction : direction);
            proj.SetPool(_isSupportWeapon ?  _pools[0] :_poolsDic[_currentData.WeaponType]);
            proj.SetSpeed(_currentData.ProjectileSpeed);
            proj.SetDamage(_currentData.Damage);

            if(_currentData.WeaponType == WeaponType.Rocket)
                proj.GetNearestTarget();

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

 
    