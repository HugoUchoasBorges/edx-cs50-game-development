using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInput : MonoBehaviour
    {

        // ========================== Components ============================

        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }


        // ========================== Input Logic ============================

        [Header("Input and Movement")]
        [SerializeField] private bool _inputEnabled = false;
        [SerializeField] [Range(0f, 10f)] private float _speed = 3f;
        [SerializeField] [Range(0f, 1f)] private float _deadZone = 0.3f;

        private const string HoriInputName = "Horizontal";
        private const string VertInputName = "Vertical";

        private Vector2 _axisInput;

        public void EnableInput(bool value) => _inputEnabled = value;

        private void HandleInput()
        {
            _axisInput = new Vector2(Input.GetAxis(HoriInputName), Input.GetAxis(VertInputName));
            if (_axisInput.magnitude > _deadZone)
            {
                _rigidbody2D.velocity = _axisInput * _speed;
            }
            else
            {
                _rigidbody2D.velocity = Vector2.zero;
            }
        }


        // ========================== Boundaries Logic ============================

        [Header("Movement borders")]
        [SerializeField] private bool _bordersEnabled = false;
        [SerializeField] private Rect _borders;

        public void EnableBorders(bool value) => _bordersEnabled = value;

        public void SetBorders(Rect rect, bool enable = true)
        {
            _borders = rect;
            EnableBorders(enable);
        }

        private float _velocityX;
        private float _velocityY;
        private void CheckBorders()
        {
            if (!_borders.Contains(transform.position))
            {
                _velocityX = _rigidbody2D.velocity.x;
                _velocityY = _rigidbody2D.velocity.y;

                // X Movement
                if ((transform.position.x > (_borders.width / 2) && _velocityX > 0) 
                    || (transform.position.x < -(_borders.width / 2) && _velocityX < 0))
                    _velocityX = 0;

                // Y Movement
                if ((transform.position.y > (_borders.height / 2) && _velocityY > 0)
                    || (transform.position.y < -(_borders.height / 2) && _velocityY < 0))
                    _velocityY = 0;

                _rigidbody2D.velocity = new Vector2(_velocityX, _velocityY);
            }
        }

        // ========================== Unity Update ============================


        private void Update()
        {
            if (_inputEnabled) HandleInput();
        }

        private void LateUpdate()
        {
            if (_bordersEnabled) CheckBorders();
        }


        // ========================== GIZMOS - Debug ============================
        private void OnDrawGizmos()
        {
            if (!_bordersEnabled) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(new Vector3(_borders.center.x, _borders.center.y, 0.01f), new Vector3(_borders.size.x, _borders.size.y, 0.01f));
        }
    }
}