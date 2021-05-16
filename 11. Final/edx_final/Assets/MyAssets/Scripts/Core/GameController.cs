using background;
using player;
using props;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace core
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private BackgroundController _background;
        [SerializeField] private PropController _propManager;

        private void Awake()
        {
            Constants.Camera = Camera.main;
            Constants.ScreenBounds = Constants.Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Constants.Camera.transform.position.z));
        }

        private void Start()
        {
            InitGame();
        }


        // ========================== Init ============================

        private void InitGame()
        {
            _player.Init();
            _background.Init();

            // Start spawning props
            _propManager.SpawnPropsLoop(5);
        }
    }
}