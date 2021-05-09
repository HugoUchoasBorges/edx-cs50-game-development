using behaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerController : MonoBehaviour
    {
        
        [Header("Components")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("Input & Behaviors")]
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerMovementBehavior _playerMovement;
        [SerializeField] private ScreenBoundariesBehavior _screenBoundariesBehavior;


        // ========================== Components ============================

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        // ========================== Init ============================


        private void OnEnable()
        {
            InitPlayer();
        }

        private void OnDisable()
        {
            _playerInput.OnMove -= _playerMovement.Move;
        }

        private void InitPlayer()
        {
            // Screen bounds
            Vector2 _screenBorders = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _camera.transform.position.z));
            Vector3 playerSize = _spriteRenderer.bounds.size;

            _screenBoundariesBehavior.SetBorders(
                new Rect(
                    0, // x
                    0, // y
                    _screenBorders.x, // width
                    _screenBorders.y // height
                    ), 
                playerSize
                );

            // Player Input
            _playerInput.EnableInput(true);

            // Player Movement
            _playerInput.OnMove += _playerMovement.Move;
        }
    }
}