using helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace collectables
{
    public class Collectable : MonoBehaviour, IPoolable
    {
        private int _points;

        public void Init(Vector2 position, Action<Collectable> onTriggerEnter, float autoDestroyDelay = 10)
        {
            transform.position = position;

            this._onDestroy = onTriggerEnter;
            _destroyCoroutine = StartCoroutine(DestroyCoroutine(autoDestroyDelay));
        }


        // ========================== Collision ============================
        
        private Action<Collectable> _onDestroy = null;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy();
        }


        // ========================== Destroy ============================

        private Coroutine _destroyCoroutine = null;

        public void Destroy()
        {
            _onDestroy?.Invoke(this);
            _onDestroy = null;

            KillDestroyCoroutine();
        }

        private IEnumerator DestroyCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
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