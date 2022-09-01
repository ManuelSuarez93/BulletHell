    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Player
{

    public class PlayerMove : MonoBehaviour
    { 
        [SerializeField] private Button _btn;
        [SerializeField] private Transform _player;
        [SerializeField] private float _offset;
        [SerializeField] private float _sensitivity = 5f;

        private PlayerInput _playerInput;
        private bool _btnPressed;

        public void ButtonPressed() => _btnPressed = true;
        public void ButtonNotPressed() => _btnPressed = false;

        private void Start()
        { 
            _playerInput = new PlayerInput();
            _playerInput.Enable();
        }
        void Update()
        {
            if (_btnPressed)
            {
                var mousePos = Vector3.zero;
            
                #if PLATFORM_ANDROID
                            mousePos = _playerInput.Move.TouchPosition.ReadValue<Vector2>(); 
                #else
                            mousePos = _playerInput.Move.MousePosition.ReadValue<Vector2>();
                #endif

                var playerPos = Camera.main.ScreenToWorldPoint(mousePos) * _sensitivity;

                _player.position = new Vector3(playerPos.x, 1f, playerPos.z + _offset);
                transform.position = new Vector3(mousePos.x, mousePos.y);
            }
        }

    }

}