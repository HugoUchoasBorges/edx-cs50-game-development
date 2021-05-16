using helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace props
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Prop : MonoBehaviour, IPoolable
    {
        private Rigidbody2D _rigidbody2d;
        private void Awake()
        {
            _rigidbody2d = GetComponent<Rigidbody2D>();
        }

        public void StartProp(Vector2 position, Vector2 velocity, Action<Prop> onCollisionEnter, float torque = 0)
        {
            transform.position = position;
            _rigidbody2d.velocity = velocity;
            _rigidbody2d.AddTorque(torque);
            this.onCollisionEnter = onCollisionEnter;
        }


        // ========================== Collision ============================

        public Action<Prop> onCollisionEnter = null;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            onCollisionEnter?.Invoke(this);
        }
    }
}