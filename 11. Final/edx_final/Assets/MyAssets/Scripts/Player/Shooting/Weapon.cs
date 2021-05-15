
using helpers;
using System.Collections.Generic;
using UnityEngine;

namespace player.shooting
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Weapon : MonoBehaviour, IPoolable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        // ========================== Bullet pool logic ============================

        private Pool<Bullet> _bullets;

        private void Awake()
        {
            _bullets = new Pool<Bullet>(30, "Prefabs/Bullet");
        }

        // ========================== Fire ============================

        [Header("Parameters")]
        [SerializeField] [Range(1f, 30f)] private float _bulletSpeed = 10f;
        [SerializeField] [Range(1f, 10f)] private float _fireRate = 5f;

        public void Fire()
        {
            KillUnlockChargeFireCoroutine();
            _unlockChargeFireCoroutine = StartCoroutine(UnlockChargeFireCoroutine());
            _chargeLock = true;

            Bullet bullet = _bullets.Instantiate();
            if (bullet != null)
            {
                KillChargeFireCoroutine();
                FireBullet(bullet);
            }
        }

        private void FireBullet(Bullet bullet)
        {
            bullet.transform.SetParent(transform, false);
            bullet.Fire(_bulletSpeed, DestroyBullet);
        }


        // ========================== Charge Fire ============================

        private Coroutine _chargeFireCoroutine = null;
        private Coroutine _unlockChargeFireCoroutine = null;
        private bool _chargeLock = false;

        public void ChargeFire()
        {
            if (_chargeLock == true || _chargeFireCoroutine != null)
                return;

            Fire();
            _chargeFireCoroutine = StartCoroutine(ChargeFireCoroutine());
        }

        private System.Collections.IEnumerator ChargeFireCoroutine()
        {
            yield return new WaitForSeconds(1 / _fireRate);
            Fire();
        }

        private System.Collections.IEnumerator UnlockChargeFireCoroutine()
        {
            yield return new WaitForSeconds(1 / _fireRate);
            _chargeLock = false;
        }

        private void KillChargeFireCoroutine()
        {
            if (_chargeFireCoroutine != null)
            {
                StopCoroutine(_chargeFireCoroutine);
                _chargeFireCoroutine = null;
            }
        }

        private void KillUnlockChargeFireCoroutine()
        {
            if (_unlockChargeFireCoroutine != null)
            {
                StopCoroutine(_unlockChargeFireCoroutine);
                _unlockChargeFireCoroutine = null;
            }
        }


        // ========================== Destroy ============================


        private void DestroyBullet(Bullet bullet)
        {
            _bullets.Destroy(bullet);
        }
    }
}
