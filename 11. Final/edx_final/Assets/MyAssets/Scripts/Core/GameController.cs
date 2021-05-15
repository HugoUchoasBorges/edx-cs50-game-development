using background;
using player;
using props;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace core
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private BackgroundController _background;
        [SerializeField] private PropController _propManager;

        private void Start()
        {
            InitGame();
        }


        // ========================== Init ============================

        private void InitGame()
        {
            _player.Init();
            _background.Init();


            _propManager.SpawnProp();
        }
    }
}