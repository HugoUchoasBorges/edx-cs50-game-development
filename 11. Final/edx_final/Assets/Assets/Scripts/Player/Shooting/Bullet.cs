using helpers;
using System;
using System.Collections;
using UnityEngine;

namespace player.shooting
{
    [RequireComponent(typeof(Rigidbody2D))]
    class Bullet : MonoBehaviour, IPoolable
    {
        private const float DESTROY_AFTER = 5f;

        // ========================== Components ============================

        Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }


        // ========================== Fire ============================

        private Action<Bullet> _onDestroy = null;

        public void Fire(float speed = 1, Action<Bullet> onDestroy = null)
        {
            _rigidbody2D.velocity = Vector2.up * speed;
            _onDestroy = onDestroy;
            _destroyCoroutine = StartCoroutine(DestroyCoroutine(speed));
        }


        // ========================== Destroy ============================

        private Coroutine _destroyCoroutine = null;

        public void Destroy()
        {
            _onDestroy?.Invoke(this);
            _onDestroy = null;

            KillDestroyCoroutine();
        }

        private IEnumerator DestroyCoroutine(float speed)
        {
            yield return new WaitForSeconds(10f / speed);
            Destroy();
        }

        private void KillDestroyCoroutine()
        {
            if (_destroyCoroutine != null)
            {
                StopCoroutine(_destroyCoroutine);
                _destroyCoroutine = null;
            }
        }
    }
}
