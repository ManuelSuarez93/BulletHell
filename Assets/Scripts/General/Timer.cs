
using System;
using UnityEngine;

namespace General
{
    public class Timer : MonoBehaviour
    {
        private float _currentTime = 0f;
        [SerializeField] private float _time = 0f;
        private bool _isStopped = false;
        private Action _onTimerAction, _onFinishAction; 
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
                    _onTimerAction?.Invoke();
                    _currentTime += Time.deltaTime;
                }
                else
                {
                    _onFinishAction?.Invoke();
                    _currentTime = 0f;
                }
            }
        }

        public void SetTimer(float time) => _time = time; 
        public void Stop(bool stop) => _isStopped = stop; 
        public void RestartTimer() => _currentTime = 0f;
        public void SetFinishAction(Action action) => _onFinishAction = action;
        public void SetUpdateAction(Action action) => _onTimerAction = action;

    }
}
