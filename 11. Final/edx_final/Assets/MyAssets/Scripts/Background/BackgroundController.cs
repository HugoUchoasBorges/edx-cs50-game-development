using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace background
{
    public class BackgroundController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private ParticleSystem _bgParticles;

        // ========================== Init ============================

        public void Init()
        {
            _bgParticles.Play();
        }
    }
}