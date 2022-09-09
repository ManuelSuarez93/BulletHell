using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using Pooling;
using System.Collections.Generic;
using System.Collections;
using static General.Global;
using System;

namespace General
{
    public class Projectile: MonoBehaviour, IMovable, IPoolable
    { 
        #region Fields
        [SerializeField] bool _destroyOnEnd = true, _isMoving = true, _followsTarget = false, _followsPlayer = false;
        [SerializeField] float _damageRate;
        [SerializeField] LayerMask _enemyLayer, _endOfLevelLayer;
        private float _speed;
        private Direction _direction;
        private int _damage;
        private PrefabPool _pool;  
        private Transform _target;
        private Timer _timer;
        private bool _damageOnStay = false;
        private List<Health> _objectsToDamage = new List<Health>();
        private bool _isLookingForTarget;
        #endregion

        #region Properties 
        public void SetDirection(Direction newDirection) => _direction = newDirection;  
        public void SetTarget(Transform newTarget) => _target = newTarget;
        public void SetPool(PrefabPool newPool) => _pool = newPool;
        public void SetDamage(int damage) => _damage = damage;
        public void SetSpeed(float speed) => _speed = speed;
        #endregion
        public void ReturnObjectToPool() => _pool.ReturnPrefab(this.gameObject, true);

        #region  Unity Methods
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
                if(_followsTarget)
                {
                    if(_target)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _speed);
                        transform.LookAt(_target); 
                    }
                    else 
                    {
                        if(!_isLookingForTarget)
                            GetNearestTarget(); 

                        transform.Translate(VectorDirection(_direction) * Time.deltaTime * _speed);
                    }
                } 
                  
                else if (_followsPlayer) 
                    transform.position = GameManager.Instance.PlayerTransform.position; 
                else
                    transform.Translate(VectorDirection(_direction) * Time.deltaTime * _speed); 
            }
        }
        #endregion
        
        #region On Trigger methods
        void OnTriggerStay(Collider other)
        { 
            if(!_destroyOnEnd && _damageOnStay && ((_enemyLayer & 1) << other.gameObject.layer) > 0)  
            { 
                other.GetComponent<Health>()?.RemoveHealth(_damage); 
                _damageOnStay = false;
            }
        } 

        private void OnTriggerEnter(Collider other)
        {
            if ((_enemyLayer & 1 << other.gameObject.layer) > 0  ) 
            {
                other.GetComponent<Health>()?.RemoveHealth(_damage);

                if(_destroyOnEnd) ReturnObjectToPool(); 
                else _objectsToDamage.Add(other.GetComponent<Health>());

            }
            else if ((_endOfLevelLayer.value & 1 << other.gameObject.layer) > 0  ) 
                if(_destroyOnEnd) ReturnObjectToPool();  
        }

        private void OnTriggerExit(Collider other) => _objectsToDamage.Remove(other.GetComponent<Health>());
        #endregion

        #region Coroutines

        public void GetNearestTarget() => StartCoroutine(GetNearestTargetRoutine());
        
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

        IEnumerator GetNearestTargetRoutine()
        {   
            _isLookingForTarget = true;
            Debug.Log("Finding nearest target!");
            while(!_target)
            {
                RaycastHit hit;
                Physics.SphereCast(transform.position, 10f, transform.forward, out hit, 10f, _enemyLayer, QueryTriggerInteraction.UseGlobal);
                if(hit.collider != null) 
                {
                    _target = hit.collider.transform; 
                    _isLookingForTarget = false;
                    Debug.Log("Target found!");
                }
                yield return null;
            }

            Debug.Log("Exiting finding target");
        }
        
        #endregion
    }

}

