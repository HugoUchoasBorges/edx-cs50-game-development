using collectables;
using helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace props
{
    public class PropController : MonoBehaviour
    {
        private const int _propCountMax = 5;

        private Pool<Prop> _propsBig;
        private Pool<Prop> _propsSmall;
        private Pool<PropExplosion> _propsExplosion;
        private Pool<PropExplosion> _propsExplosionSmall;
        private Pool<Collectable> _collectables;

        private void Awake()
        {
            _propsBig = new Pool<Prop>(_propCountMax, "Prefabs/Prop");
            _propsExplosion = new Pool<PropExplosion>(_propCountMax, "Prefabs/PropExplosion");

            _propsSmall = new Pool<Prop>(_propCountMax * 2, "Prefabs/PropSmall");
            _propsExplosionSmall = new Pool<PropExplosion>(_propCountMax * 2, "Prefabs/PropExplosionSmall");

            _collectables = new Pool<Collectable>(_propCountMax * 8, "Prefabs/Collectable");
        }

        public void SpawnProp()
        {
            GenerateBigProp(new Vector2(0, 3), new Vector2(0, -.5f), 180);
        }


        // ========================== Big Props ============================

        private void GenerateBigProp(Vector2 position, Vector2 velocity, float torque = 0)
        {
            _propsBig.Instantiate(transform).StartProp(position, velocity, DestroyBigProp, torque);
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

            _propsBig.Destroy(prop);
        }


        // ========================== Small Prop ============================

        private void GenerateSmallProp(Vector2 position, Vector2 velocity, float torque = 0)
        {
            _propsSmall.Instantiate(transform).StartProp(position, velocity, DestroySmallProp, torque);
        }

        private void DestroySmallProp(Prop prop)
        {
            Vector2 propPosition = prop.transform.position;

            GenerateSmallExplosion(propPosition);
            GenerateCollectables(propPosition, 2);

            _propsSmall.Destroy(prop);
        }


        // ========================== Explosion ============================

        private void GenerateBigExplosion(Vector2 position)
        {
            _propsExplosion.Instantiate(transform).Play(position, (obj) => _propsExplosion.Destroy(obj));
        }

        private void GenerateSmallExplosion(Vector2 position)
        {
            _propsExplosionSmall.Instantiate(transform).Play(position, (obj) => _propsExplosion.Destroy(obj));
        }

        // ========================== Collectables ============================

        private void GenerateCollectables(Vector2 position, int maxPoints)
        {
            int points = Random.Range(0, maxPoints + 1);
            for (int i = 0; i < points; i++)
            {
                _collectables.Instantiate(transform).Init(position + GetRandomVector2(.4f, .4f), OnCollectableCollided);
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