using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using General;
using Pooling;

namespace Player
{
    [System.Serializable]
    public class Special
    {
        [SerializeField] private PrefabPool _pool; 
        [SerializeField] private Timer _timer;
        private bool _canUseSpecial; 
        private GameObject _projectile;
        List<SpecialData> _specialData;
        SpecialData _currentData; 

        private void Shotgun(Transform origin, Vector3 vectorDirection)
        {
            var angleDifference = _currentData.ProjectileAmount > 1 ? (180f / (_currentData.ProjectileAmount - 1)) : 0;
            var totalAngle = -90f;
            for (int i = 0; i < _currentData.ProjectileAmount; i++)
            {
                var x = _pool.GetPrefab(true);
                x.transform.localPosition = origin.localPosition;
                if (_currentData.ProjectileAmount > 1)
                {
                    x.transform.localEulerAngles = new Vector3(0, totalAngle, 0);
                    totalAngle += angleDifference;
                }
                var proj = x.GetComponent<Projectile>();
                proj.SetDirection(vectorDirection);
                proj.SetPool(_pool);
                proj.SetSpeed(_currentData.ProjectileSpeed);
                proj.SetDamage(_currentData.Damage);
            }
        }

        public void UseSpecialcial(Transform origin, Vector3 vectorDirection, SpecialType type)
        {   
            if(!_canUseSpecial) return;

            switch(type)
            {
                case SpecialType.Shotgun: Shotgun(origin, vectorDirection); break;
                case SpecialType.Explosion: Shotgun(origin, vectorDirection); break;
                case SpecialType.Laser: Shotgun(origin, vectorDirection); break;
            }

            _canUseSpecial = false;
            _timer.Stop(false);
            
        }

        public void InitializeSpecial() 
        {
            _specialData = 
                Resources
                .LoadAll<SpecialData>("Specials")
                .ToList(); 
         
            _currentData = _specialData?[0];
            _timer.SetFinishAction(() => 
            {
                _canUseSpecial = true;
                _timer.Stop(true); 
            });

            _timer.SetUpdateAction(() => UIManager.Instance.SetCooldownSpecial(_timer.TimePcnt)); 
            
            _timer.SetTimer(_currentData.CooldownTime);
            _timer.Stop(false);
            

        }   

        public void SetSpecial(SpecialType type)
        {   
            var data = _specialData.FirstOrDefault(x => x.Type == type);

            if(data!= null) 
                _currentData = data; 
            else 
                Debug.LogError($"Error: no weapon data found for type:{type}"); 

        }
 
    } 

}
 


    