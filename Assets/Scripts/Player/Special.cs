using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static General.Global;
using General;
using Pooling;
using System.Collections;

namespace Player
{
    [System.Serializable]
    public class Special
    { 
        [SerializeField] private Timer _timer;
        [SerializeField] private List<PrefabPool> _pools; 
        private Dictionary<SpecialType, PrefabPool> _poolsDic;
        private bool _canUseSpecial; 
        private GameObject _projectile;
        List<SpecialData> _specialData;
        SpecialData _currentData; 


        public void InitializeSpecial() 
        {
            //Get special data
            _specialData = LoadResources<SpecialData>(SPECIAL_FOLDER);
         
            _currentData = _specialData?[0];
            _timer.SetFinishAction(() => 
            {
                _canUseSpecial = true;
                _timer.Stop(true); 
            });

            //Set timer
            _timer.SetUpdateAction(() => UIManager.Instance.SetCooldownSpecial(_timer.TimePcnt)); 
            
            _timer.SetTimer(_currentData.CooldownTime);
            _timer.Stop(false);
            
            //Set prefab pools
            _poolsDic = new Dictionary<SpecialType, PrefabPool>(); 

            if( _pools.Count  < Enum.GetValues(typeof(SpecialType)).Length)
                 Debug.LogError("Not enough pools for each special type");
            else
            { 
                for (int i = 0; i < _pools.Count; i++)
                { 
                    SpecialType type = (SpecialType)Enum.GetValues(typeof(SpecialType)).GetValue(i);
                    _pools[i].gameObject.name = type.ToString();
                    _pools[i].SetPrefab(_specialData.FirstOrDefault(x => x.Type == type).Projectile);
                    _pools[i].CreatePrefabs();
                    _poolsDic.Add(type,_pools[i]);
                }
            } 
        }   

        public void SetSpecial(SpecialType type)
        {   
            var data = _specialData.FirstOrDefault(x => x.Type == type);

            if(data!= null) 
            {
                _currentData = data; 
                _timer.SetTimer(_currentData.CooldownTime);
            }
            else 
                Debug.LogError($"Error: no weapon data found for type:{type}"); 

        }
         
        public void UseSpecialcial(Transform origin, Vector3 vectorDirection, SpecialType type)
        {   
            if(!_canUseSpecial) return;

            switch(type)
            {
                case SpecialType.Shotgun: Shotgun(origin, vectorDirection); break;
                case SpecialType.Explosion: Shotgun(origin, vectorDirection); break;
                case SpecialType.Laser: Laser(origin, vectorDirection); break;
            } 
            _canUseSpecial = false;
            _timer.Stop(false); 
        }
        
        #region Special methods
        private void Shotgun(Transform origin, Vector3 vectorDirection)
        {
            var angleDifference = _currentData.ProjectileAmount > 1 ? (180f / (_currentData.ProjectileAmount - 1)) : 0;
            var totalAngle = -90f;
            for (int i = 0; i < _currentData.ProjectileAmount; i++)
            {
                var x = _poolsDic[SpecialType.Shotgun].GetPrefab(true);
                x.transform.localPosition = origin.localPosition;
                if (_currentData.ProjectileAmount > 1)
                {
                    x.transform.localEulerAngles = new Vector3(0, totalAngle, 0);
                    totalAngle += angleDifference;
                }
                var proj = x.GetComponent<Projectile>();
                proj.SetDirection(vectorDirection);
                proj.SetPool(_poolsDic[SpecialType.Shotgun]);
                proj.SetSpeed(_currentData.ProjectileSpeed);
                proj.SetDamage(_currentData.Damage);
            }
        }

        private void Laser(Transform origin, Vector3 vectorDirection)
        {
            var x = _poolsDic[SpecialType.Laser].GetPrefab(true);
            x.transform.localPosition = origin.localPosition; 
            var proj = x.GetComponent<Projectile>();
            proj.SetDirection(vectorDirection);  
            proj.SetDamage(_currentData.Damage);
            proj.SetPool(_poolsDic[SpecialType.Laser]);
            proj.SetFollow(GameManager.Instance.PlayerTransform);
            var timer = proj.GetComponent<Timer>();
            timer.SetFinishAction(() => proj.ReturnObjectToPool());
                proj.SetPool(_poolsDic[SpecialType.Laser]);
        }
        #endregion
 
    } 

}
 


    