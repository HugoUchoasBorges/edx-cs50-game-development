using background;
using enemy;
using player;
using props;
using UnityEngine;
using util;

namespace core
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private UnityEngine.UI.CanvasScaler _canvasScaler;
        [SerializeField] private PlayerController _player;
        [SerializeField] private BackgroundController _background;
        [SerializeField] private PropController _propManager;
        [SerializeField] private EnemyManager _enemyManager;

        private void Awake()
        {
            Camera cam = Constants.Camera = Camera.main;
            Vector2 bounds = Constants.Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
            Constants.ScreenBounds = new Vector2(bounds.y * _canvasScaler.referenceResolution.x / _canvasScaler.referenceResolution.y, bounds.y);
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

            // Start spawning enemies
            _enemyManager.SpawnEnemy();
        }
    }
}