using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {    
        [SerializeField] Health _health; 
        [SerializeField] LayerMask _collisionLayer;  
         
        private void Start()
        {
            UIManager.Instance.SetHealth(_health.HealthPcnt);
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((_collisionLayer & 1 << other.gameObject.layer) > 0)
            {
                _health.RemoveHealth(1);
                
                UIManager.Instance.SetHealth(_health.HealthPcnt);
            } 
        }

        public void SwitchShip()
        {

        }
        
    }
}

