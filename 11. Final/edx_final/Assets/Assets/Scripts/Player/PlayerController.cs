using behaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private ScreenBoundariesBehavior _screenBoundariesBehavior;
        [SerializeField] private SpriteRenderer _spriteRenderer;


        // ========================== Components ============================

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        // ========================== Init ============================


        private void Start()
        {
            InitPlayer();
        }

        private void InitPlayer()
        {
            Vector2 _screenBorders = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _camera.transform.position.z));
            Vector3 playerSize = _spriteRenderer.bounds.size;

            _playerInput.EnableInput(true);
            _screenBoundariesBehavior.SetBorders(
                new Rect(
                    0, // x
                    0, // y
                    _screenBorders.x, // width
                    _screenBorders.y // height
                    ), 
                playerSize
                );
        }
    }
}