using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace background
{
    public class BackgroundController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private ParticleSystem _bgParticles;

        private void Start()
        {
            InitBackground();
        }


        // ========================== Init ============================

        private void InitBackground()
        {
            _bgParticles.Play();
        }
    }
}