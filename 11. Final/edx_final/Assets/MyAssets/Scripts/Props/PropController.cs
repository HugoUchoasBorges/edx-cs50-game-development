using collectables;
using helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace props
{
    public class PropController : MonoBehaviour
    {
        private const int _propCountMax = 5;

        private Pool<Prop> _propsBig;
        private Pool<Prop> _propsSmall;
        private Pool<Collectable> _collectables;

        private void Awake()
        {
            _propsBig = new Pool<Prop>(_propCountMax, "Prefabs/Prop");
            _propsSmall = new Pool<Prop>(_propCountMax * 2, "Prefabs/PropSmall");
            _propsSmall = new Pool<Prop>(_propCountMax * 2, "Prefabs/PropSmall");
            _collectables = new Pool<Collectable>(_propCountMax * 8, "Prefabs/Collectable");
        }

        public void SpawnProp()
        {
            _propsBig.Instantiate(transform).StartProp(new Vector2(0, 1), Vector2.zero, OnBigPropCollided, 0);
        }

        private void OnBigPropCollided(Prop prop)
        {
            _collectables.Instantiate(transform).Init(prop.transform.position, OnCollectableCollided);

            _propsSmall.Instantiate(transform).StartProp((Vector2)prop.transform.position + Vector2.one * 1, Vector2.zero, OnSmallPropCollided, 0);
            _propsSmall.Instantiate(transform).StartProp((Vector2)prop.transform.position + Vector2.one * -1, Vector2.zero, OnSmallPropCollided, 0);
            _propsBig.Destroy(prop);
        }

        private void OnSmallPropCollided(Prop prop)
        {
            _collectables.Instantiate(transform).Init(prop.transform.position, OnCollectableCollided);

            _propsSmall.Destroy(prop);
        }

        private void OnCollectableCollided(Collectable collectable)
        {
            _collectables.Destroy(collectable);
        }
    }
}