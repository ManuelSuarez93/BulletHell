using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using Pooling;
using System.Collections.Generic;
using System.Collections;
using static General.Global;

namespace General
{
    public class Projectile: MonoBehaviour, IMovable, IPoolable
    { 
        [SerializeField] bool _destroyOnEnd = true, _isMoving = true, _follosTarget = false;
        [SerializeField] float _damageRate;
        [SerializeField] LayerMask _collisionLayer;
        [SerializeField] LayerMask _endOfLevelLayer;
        private float _speed;
        private Direction _direction;
        private int _damage;
        private PrefabPool _pool;  
        private Transform _follow;
        private Timer _timer;
        private bool _damageOnStay = false;
        private List<Health> _objectsToDamage = new List<Health>();

        #region Properties 
        public void SetDirection(Direction newDirection) => _direction = newDirection;  
        public void SetFollow(Transform newFollow) => _follow = newFollow;
        public void SetPool(PrefabPool newPool) => _pool = newPool;
        public void SetDamage(int damage) => _damage = damage;
        public void SetSpeed(float speed) => _speed = speed;
        #endregion
        void OnEnable() => StartCoroutine(DamageByTime());
        void OnDisable() => StopCoroutine(DamageByTime());
        private void Awake()
        {
            if(!_destroyOnEnd)
            {
                var _timerObj = new GameObject();
                _timerObj.transform.parent = transform;
                _timer = _timerObj.AddComponent<Timer>();
                _timer.Stop(false);
                _timer.SetTimer(_damageRate);
                _timer.SetFinishAction(()=>{_damageOnStay = true;});
            }
        }

        private void Update()
        {
            if(_isMoving)
            {
                if(_follosTarget)
                    transform.position = _follow.position;
                else
                    transform.Translate(VectorDirection(_direction) * Time.deltaTime * _speed);

            }
        }

        void OnTriggerStay(Collider other)
        { 
            if(!_destroyOnEnd && _damageOnStay && ((_collisionLayer & 1) << other.gameObject.layer) > 0)  
            { 
                other.GetComponent<Health>()?.RemoveHealth(_damage); 
                _damageOnStay = false;
            }
        } 
        private void OnTriggerEnter(Collider other)
        {
            if ((_collisionLayer & 1 << other.gameObject.layer) > 0  ) 
            {
                other.GetComponent<Health>()?.RemoveHealth(_damage);

                if(_destroyOnEnd) ReturnObjectToPool(); 
                else _objectsToDamage.Add(other.GetComponent<Health>());

            }
            else if ((_endOfLevelLayer.value & 1 << other.gameObject.layer) > 0  ) 
                if(_destroyOnEnd) ReturnObjectToPool();  
        }

        void OnTriggerExit(Collider other) => _objectsToDamage.Remove(other.GetComponent<Health>());
        public void ReturnObjectToPool() => _pool.ReturnPrefab(this.gameObject, true);
        
        IEnumerator DamageByTime()
        { 
            while(true)
            {
                if(_damageOnStay)
                { 
                    foreach(var h in _objectsToDamage)
                    { 
                        h?.RemoveHealth(_damage); 
                        Debug.Log($"Doing damage to:<color:blue>{h.gameObject.name}</color> ");
                    }
                    _damageOnStay = false;
                }
                yield return null;
            }
        }


        
    }

}

