using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;


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
            Vector2 _screenBorders = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            _playerInput.EnableInput(true);
            _playerInput.SetBorders(new Rect(-_screenBorders.x, -_screenBorders.y, _screenBorders.x * 2, _screenBorders.y * 2));
        }
    }
}