using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace behaviors
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ScreenBoundariesBehavior : MonoBehaviour
    {
        // ========================== Components ============================

        [Header("Movement borders")]
        [SerializeField] private bool _bordersEnabled = false;
        [SerializeField] private Rect _borders;

        private Vector2 _objectSize = Vector2.zero;

        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }


        // ========================== Borders ============================

        public Action OnBorderReached = null;

        public void EnableBorders(bool value) => _bordersEnabled = value;

        public void SetBorders(Rect rect, Vector2 objectSize, bool enable = true)
        {
            SetBorders(rect, enable);
            _objectSize = objectSize;
            _borders.width -= objectSize.x / 2;
            _borders.height -= objectSize.y / 2;
        }

        public void SetBorders(Rect rect, bool enable = true)
        {
            _borders = rect;
            EnableBorders(enable);
        }

        private bool _borderReached;
        private float _velocityX;
        private float _velocityY;
        private void CheckBorders()
        {
            _borderReached = false;

            _velocityX = _rigidbody2D.velocity.x;
            _velocityY = _rigidbody2D.velocity.y;

            // X Movement
            if (_velocityX > 0 && transform.position.x > (_borders.x + _borders.width))
            {
                _velocityX = 0;
                _borderReached = true;
            }
            else if (_velocityX < 0 && transform.position.x < (_borders.x - _borders.width))
            {
                _velocityX = 0;
                _borderReached = true;
            }

            // Y Movement
            if (_velocityY > 0 && transform.position.y > (_borders.y + _borders.height))
            {
                _velocityY = 0;
                _borderReached = true;
            }
            else if (_velocityY < 0 && transform.position.y < (_borders.y - _borders.height))
            {
                _velocityY = 0;
                _borderReached = true;
            }

            _rigidbody2D.velocity = new Vector2(_velocityX, _velocityY);

            if (_borderReached) OnBorderReached?.Invoke();
        }


        // ========================== Unity Update ============================

        private void LateUpdate()
        {
            if (_bordersEnabled) CheckBorders();
        }


        // ========================== GIZMOS - Debug ============================
        private void OnDrawGizmos()
        {
            if (!_bordersEnabled) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(new Vector3(_borders.x, _borders.y, 0.01f), new Vector3(_objectSize.x + _borders.width * 2, _objectSize.y + _borders.height * 2, 0.01f));

            Gizmos.color = Color.gray;
            Gizmos.DrawWireCube(new Vector3(_borders.x, _borders.y, 0.01f), new Vector3(_borders.width * 2, _borders.height * 2, 0.01f));
        }
    }
}