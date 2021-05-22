using helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;

        private Pool<EnemyController> _enemies;

        private void Awake()
        {
            if (_prefab != null)
                _enemies = new Pool<EnemyController>(10, _prefab);
            else
                _enemies = new Pool<EnemyController>(10, Constants.PREFAB_ENEMY);
        }


        // ========================== Spawn ============================

        public void SpawnEnemy()
        {
            EnemyController enemy = _enemies.Instantiate(transform);
            enemy.transform.position = Vector2.one;
            enemy.Init();
        }
    }
}