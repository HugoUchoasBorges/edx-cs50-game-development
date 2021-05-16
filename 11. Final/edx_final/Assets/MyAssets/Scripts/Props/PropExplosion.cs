using helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace props
{
    public class PropExplosion : MonoBehaviour,IPoolable
    {
        [SerializeField] private ParticleSystem _explosionParticles;

        private Action<PropExplosion> _onDestroy;

        public void Play(Vector2 position, Action<PropExplosion> onDestroy)
        {
            transform.position = position;
            _onDestroy = onDestroy;
            _explosionParticles.Play();
        }

        private void OnParticleSystemStopped()
        {
            _onDestroy.Invoke(this);
        }
    }
}