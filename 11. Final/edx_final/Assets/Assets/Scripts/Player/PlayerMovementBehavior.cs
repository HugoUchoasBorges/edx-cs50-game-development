
using UnityEngine;

namespace player
{
    [RequireComponent(typeof(Rigidbody2D))]
    class PlayerMovementBehavior : MonoBehaviour
    {
        // ========================== Components ============================

        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        // ========================== Movement Logic ============================

        [SerializeField] [Range(0f, 10f)] private float _speed = 3f;
        [SerializeField] [Range(0f, 1f)] private float _deadZone = 0.3f;

        public void Move(Vector2 input)
        {
            if (input.magnitude > _deadZone)
            {
                _rigidbody2D.velocity = Vector2.ClampMagnitude(input, 1) * _speed;
            }
            else
            {
                _rigidbody2D.velocity = Vector2.zero;
            }
        }
    }
}
