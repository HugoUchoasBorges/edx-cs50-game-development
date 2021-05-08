using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    [RequireComponent(typeof(Rigidbody2D))]
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

        // ========================== Unity Update ============================

        private void Update()
        {
            if (_inputEnabled) HandleInput();
        }
    }
}