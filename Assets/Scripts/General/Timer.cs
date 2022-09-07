
using System;
using UnityEngine;
using UnityEngine.Events;

namespace General
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private float _time = 0f;
        [SerializeField] private bool _destroyOnFinish = false, 
                                    _useUnityFinishEvent = false,
                                    _useUnityEventUpdate = false;
        [SerializeField] private UnityEvent _onFinishEvent, _onUpdateEvent;
        private float _currentTime = 0f;
        private bool _isStopped = false;
        private Action _onUpdateAction, _onFinishAction; 
        public bool Stopped => _isStopped;
        public float TimePcnt => _currentTime/ _time;

        void Start()
        {
            _currentTime = 0f;
        }
        void Update()
        {
            if(!_isStopped)
            {   
                if(_currentTime < _time) 
                { 
                    _onUpdateAction?.Invoke();
                    if(_useUnityFinishEvent) 
                        _onUpdateEvent.Invoke();
                    _currentTime += Time.deltaTime;
                }
                else
                {
                    _onFinishAction?.Invoke();
                    if(_useUnityFinishEvent) 
                        _onFinishEvent.Invoke();
                    if(_destroyOnFinish) 
                        Destroy();

                    _currentTime = 0f;
                    
                }
            }
        }

        public void SetTimer(float time) => _time = time; 
        public void Stop(bool stop) => _isStopped = stop; 
        public void RestartTimer() => _currentTime = 0f;
        public void SetFinishAction(Action action) => _onFinishAction = action;
        public void SetUpdateAction(Action action) => _onUpdateAction = action;
        public void Destroy() => Destroy(gameObject);
    }
}
