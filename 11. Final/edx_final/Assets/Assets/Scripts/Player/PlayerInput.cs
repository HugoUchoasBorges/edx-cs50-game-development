using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class PlayerInput : MonoBehaviour
    {
        // ========================== Callbacks logic ============================

        public event Action<Vector2> OnMove = delegate { };

        // ========================== Input Logic ============================

        [SerializeField] private bool _inputEnabled = false;

        private const string HoriInputName = "Horizontal";
        private const string VertInputName = "Vertical";

        private Vector2 _axisInput;

        public void EnableInput(bool value) => _inputEnabled = value;

        private void HandleInput()
        {
            _axisInput = new Vector2(Input.GetAxis(HoriInputName), Input.GetAxis(VertInputName));
            OnMove(_axisInput);
        }

        // ========================== Unity Update ============================

        private void Update()
        {
            if (_inputEnabled) HandleInput();
        }
    }
}