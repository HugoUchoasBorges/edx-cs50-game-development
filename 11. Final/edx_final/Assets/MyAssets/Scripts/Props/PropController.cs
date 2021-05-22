using collectables;
using helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using util;

namespace props
{
    public class PropController : MonoBehaviour
    {
        private const int PROP_MAX_COUNT = 10;
        private const int PROP_MAX_ANGLE = 30;
        private const int PROP_MAX_TORQUE = 90;

        private Pool<Prop> _propsBig;
        private Pool<Prop> _propsSmall;
        private Pool<PropExplosion> _propsExplosion;
        private Pool<PropExplosion> _propsExplosionSmall;
        private Pool<Collectable> _collectables;

        private void Awake()
        {
            _propsBig = new Pool<Prop>(PROP_MAX_COUNT, Constants.PREFAB_PROP);
            _propsExplosion = new Pool<PropExplosion>(PROP_MAX_COUNT, Constants.PREFAB_PROP_EXPLOSION);

            _propsSmall = new Pool<Prop>(PROP_MAX_COUNT * 2, Constants.PREFAB_PROP_SMALL);
            _propsExplosionSmall = new Pool<PropExplosion>(PROP_MAX_COUNT * 2, Constants.PREFAB_PROP_EXPLOSION_SMALL);

            _collectables = new Pool<Collectable>(PROP_MAX_COUNT * 8, Constants.PREFAB_COLLECTABLE);
        }


        // ========================== Spawn ============================

        private Coroutine _spawnPropsCoroutine = null;
        [SerializeField] private Transform[] _spawnPoints;

        public void SpawnProp()
        {
            Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

            float velocity = Random.Range(.5f, 1f);
            Vector2 direction = velocity * (Quaternion.Euler(0, 0, Random.Range(-PROP_MAX_ANGLE, PROP_MAX_ANGLE)) * spawnPoint.up);

            GenerateBigProp(spawnPoint.position, direction, Random.Range(-PROP_MAX_TORQUE, PROP_MAX_TORQUE));
        }

        public void SpawnPropsLoop(float spawnDelay, float spawnChance = 0.5f)
        {
            KillSpawnPropsCoroutine();
            _spawnPropsCoroutine = StartCoroutine(SpawnPropsCoroutine(spawnDelay, spawnChance));
        }

        public void StopSpawningProps()
        {
            KillSpawnPropsCoroutine();
        }

        private IEnumerator SpawnPropsCoroutine(float delay, float chance)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                if(Random.Range(0f, 1f) > chance)
                    SpawnProp();
            }
        }

        private void KillSpawnPropsCoroutine()
        {
            if(_spawnPropsCoroutine != null)
            {
                StopCoroutine(_spawnPropsCoroutine);
                _spawnPropsCoroutine = null;
            }
        }


        // ========================== Generic Props ============================

        private void DestroyProp(Pool<Prop> props, Prop prop)
        {
            props.Destroy(prop);
        }

        // ========================== Big Props ============================

        private void GenerateBigProp(Vector2 position, Vector2 velocity, float torque = 0)
        {
            _propsBig.Instantiate(transform)?.StartProp(position, velocity, DestroyBigProp, DestroyBigPropImmediate, torque);
        }

        private void DestroyBigProp(Prop prop)
        {
            Vector2 propPosition = prop.transform.position;
            Vector2 propVelocity = prop.Velocity + Vector2.one;

            GenerateCollectables(propPosition, 7);
            GenerateBigExplosion(propPosition);

            int smallProps = Random.Range(0, 3);
            for (int i = 0; i < smallProps; i++)
            {
                GenerateSmallProp(
                    propPosition,
                    GetRandomVector2(propVelocity.x, propVelocity.y),
                    Random.Range(-1, 1)
                    );
            }

            DestroyProp(_propsBig, prop);
        }

        private void DestroyBigPropImmediate(Prop prop)
        {
            DestroyProp(_propsBig, prop);
        }


        // ========================== Small Prop ============================

        private void GenerateSmallProp(Vector2 position, Vector2 velocity, float torque = 0)
        {
            _propsSmall.Instantiate(transform)?.StartProp(position, velocity, DestroySmallProp, DestroySmallPropImmediate, torque);
        }

        private void DestroySmallProp(Prop prop)
        {
            Vector2 propPosition = prop.transform.position;

            GenerateSmallExplosion(propPosition);
            GenerateCollectables(propPosition, 2);

            DestroyProp(_propsSmall, prop);
        }

        private void DestroySmallPropImmediate(Prop prop)
        {
            DestroyProp(_propsSmall, prop);
        }


        // ========================== Explosion ============================

        private void GenerateBigExplosion(Vector2 position)
        {
            _propsExplosion.Instantiate(transform)?.Play(position, (obj) => _propsExplosion.Destroy(obj));
        }

        private void GenerateSmallExplosion(Vector2 position)
        {
            _propsExplosionSmall.Instantiate(transform)?.Play(position, (obj) => _propsExplosion.Destroy(obj));
        }

        // ========================== Collectables ============================

        private void GenerateCollectables(Vector2 position, int maxPoints)
        {
            int points = Random.Range(0, maxPoints + 1);
            for (int i = 0; i < points; i++)
            {
                _collectables.Instantiate(transform)?.Init(position + GetRandomVector2(.4f, .4f), OnCollectableCollided);
            }
        }

        private void OnCollectableCollided(Collectable collectable)
        {
            _collectables.Destroy(collectable);
        }


        // ========================== Auxiliar Methods ============================

        private Vector2 GetRandomVector2(float rangeX, float rangeY)
        {
            return new Vector2(Random.Range(-rangeX, rangeX), Random.Range(-rangeY, rangeY));
        }
    }
}