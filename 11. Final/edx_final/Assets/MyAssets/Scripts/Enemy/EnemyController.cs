using behaviors;
using behaviors.shooting;
using helpers;
using UnityEngine;

namespace enemy
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyController : MonoBehaviour, IPoolable
    {

        [Header("Components")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("Input & Behaviors")]
        [SerializeField] private EnemyInput _enemyInput;
        [SerializeField] private MovementBehavior _enemyMovement;
        [SerializeField] private ShootingManager _enemyShootingManager;


        // ========================== Components ============================

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        // ========================== Init ============================


        private void OnEnable()
        {
            _enemyInput.OnMove += _enemyMovement.Move;
            _enemyInput.OnFirePressed += _enemyShootingManager.Fire;
            _enemyInput.OnFireHeld += _enemyShootingManager.ChargeFire;
        }

        private void OnDisable()
        {
            _enemyInput.OnMove -= _enemyMovement.Move;
            _enemyInput.OnFirePressed -= _enemyShootingManager.Fire;
            _enemyInput.OnFireHeld -= _enemyShootingManager.ChargeFire;
        }

        public void Init()
        {
            // Enemy Input
            _enemyInput.EnableInput(true);

            // Default weapon
            _enemyShootingManager.InstallWeapon(ShootingManager.WeaponPosition.LEFT);
            //_enemyShootingManager.InstallWeapon(ShootingManager.WeaponPosition.CENTER);
            _enemyShootingManager.InstallWeapon(ShootingManager.WeaponPosition.RIGHT);
        }
    }
}