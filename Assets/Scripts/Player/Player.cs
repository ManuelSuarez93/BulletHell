using System;
using System.Collections;
using System.Collections.Generic; 
using static General.Global;
using General;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        #region  Fields
        [SerializeField] private Health _health; 
        [SerializeField] private LayerMask _collisionLayer;  
        [SerializeField] private List<Ship> _ships; 
        [SerializeField] private Direction _projectileDirection; 
        [SerializeField] private Special _special;  
        [SerializeField] private float _shipSwitchSpeed;
        [SerializeField] private Weapon _weapon, _supportW1, _supportW2; 
        
        private int _currentShipIndex;  
        private int[] _supportShipIndex= new int[2];
        private float _timer;  
        
        
        private Vector3[] _shipPositions = new Vector3[3];
        #endregion 

        #region Unity Methods
        void Awake()
        {    
            for(int i=0; i < _shipPositions.Length; i++) 
                _shipPositions[i] = _ships[i].ShipObject.transform.localPosition;

            _weapon.InitializeWeapon(_ships[0]);  
            _supportW1.InitializeWeapon(_ships[0]);  
            _supportW2.InitializeWeapon(_ships[0]); 
            _weapon.SetWeapon((WeaponType)0);
            _special.InitializeSpecial();
        }

        private void Start()
        {
            UIManager.Instance.SetHealth(_health.HealthPcnt);
            _timer = 0f; 
            _currentShipIndex = 1;
            _supportShipIndex[0] = 0;
            _supportShipIndex[1] = 2;
        }

        public void Update()
        {
            _weapon.Shoot(_ships[_currentShipIndex].ShipObject.transform, VectorDirection(_projectileDirection));  
            _supportW1.Shoot(_ships[_supportShipIndex[0]].ShipObject.transform); 
            _supportW2.Shoot(_ships[_supportShipIndex[1]].ShipObject.transform); 
            
        }
        #endregion
        
        public void UseSpecial() => _special.UseSpecialcial(transform, VectorDirection(_projectileDirection), _ships[_currentShipIndex].Special);

        public void SwitchShip()
        { 
            _supportShipIndex[0] = _currentShipIndex;
            _currentShipIndex = _currentShipIndex == _ships.Count - 1 ? 0 : ++_currentShipIndex;  
            _supportShipIndex[1] = _currentShipIndex == _ships.Count - 1 ? 0 : _currentShipIndex + 1;  
            _weapon.SetWeapon(_ships[_currentShipIndex].Weapon); 
            _special.SetSpecial(_ships[_currentShipIndex].Special);
            StopCoroutine(MoveShips(0,0,0));
            StartCoroutine(MoveShips(_supportShipIndex[0], _currentShipIndex, _supportShipIndex[1])); 

        }
 
        private IEnumerator MoveShips (int oldship, int newship, int nextship)
        {
            var timer = 0f;
            while (timer < _shipSwitchSpeed)
            {
                
                var oldpos = _ships[oldship].ShipObject.transform.localPosition;
                var newpos = _ships[newship].ShipObject.transform.localPosition; 
                var nextpos = _ships[nextship].ShipObject.transform.localPosition;

                _ships[oldship].ShipObject.transform.localPosition = Vector3.Lerp(oldpos, _shipPositions[0], timer/_shipSwitchSpeed);
                _ships[newship].ShipObject.transform.localPosition = Vector3.Lerp(newpos, _shipPositions[1], timer/_shipSwitchSpeed);
                _ships[nextship].ShipObject.transform.localPosition = Vector3.Lerp(nextpos, _shipPositions[2], timer/_shipSwitchSpeed);

                timer+= Time.deltaTime;

                yield return null;
            }
 
            _ships[oldship].ShipObject.transform.localPosition =  _shipPositions[0];
            _ships[newship].ShipObject.transform.localPosition = _shipPositions[1];
            _ships[nextship].ShipObject.transform.localPosition = _shipPositions[2];

        }
        
        private void OnTriggerEnter(Collider other)
        {
            if ((_collisionLayer & 1 << other.gameObject.layer) > 0)
            {
                _health.RemoveHealth(1);
                
                UIManager.Instance.SetHealth(_health.HealthPcnt);
            } 
        }
 
    }
    
}
 