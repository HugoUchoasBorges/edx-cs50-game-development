using helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace props
{
    public class PropController : MonoBehaviour
    {
        private Pool<Prop> _props;

        private void Awake()
        {
            _props = new Pool<Prop>(30, "Prefabs/Prop");
        }

        public void SpawnProp()
        {
            Prop prop = _props.Instantiate(transform);
            prop.StartProp(Vector2.one * 2, Vector2.zero, OnPropCollided, 0);
        }

        private void OnPropCollided(Prop prop)
        {
            //_props.Destroy(prop);
        }
    }
}