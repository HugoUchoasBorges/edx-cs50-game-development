using System;
using UnityEngine;

namespace behaviors
{
    public abstract class SpaceshipInput : MonoBehaviour
    {
        // ========================== Callbacks logic ============================

        public event Action<Vector2> OnMove = delegate { };
        public event Action OnFirePressed = delegate { };
        public event Action OnFireHeld = delegate { };

        // ========================== Input Logic ============================

        [SerializeField] private bool _inputEnabled = false;

        private const string HoriInputName = "Horizontal";
        private const string VertInputName = "Vertical";

        private const string ShootInputName = "Fire";

        private Vector2 _axisInput;

        public void EnableInput(bool value) => _inputEnabled = value;

        private void HandleInput()
        {
            // Movement
            _axisInput = new Vector2(Input.GetAxis(HoriInputName), Input.GetAxis(VertInputName));
            OnMove(_axisInput);

            // Shooting
            if (Input.GetButtonDown(ShootInputName))
                OnFirePressed();
            else if (Input.GetButton(ShootInputName))
                OnFireHeld();
        }

        // ========================== Unity Update ============================

        private void Update()
        {
            if (_inputEnabled) HandleInput();
        }
    }
}