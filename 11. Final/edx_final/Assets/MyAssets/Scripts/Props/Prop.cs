using behaviors;
using helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace props
{
    [RequireComponent(typeof(ScreenBoundariesBehavior))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Prop : MonoBehaviour, IPoolable
    {
        private Rigidbody2D _rigidbody2d;
        public Vector2 Velocity => _rigidbody2d.velocity;

        private ScreenBoundariesBehavior _screenBoundariesBehavior;

        private void Awake()
        {
            _rigidbody2d = GetComponent<Rigidbody2D>();
            _screenBoundariesBehavior = GetComponent<ScreenBoundariesBehavior>();
        }

        public void StartProp(Vector2 position, Vector2 velocity, Action<Prop> onCollisionEnter, Action<Prop> onBorderReached, float torque = 0)
        {
            transform.position = position;
            _rigidbody2d.velocity = velocity;
            _rigidbody2d.AddTorque(torque);
            this.onCollisionEnter = onCollisionEnter;
            this.onBorderReached = onBorderReached;

            // Screen bounds
            _screenBoundariesBehavior.SetBorders(
                new Rect(
                    0, // x
                    0, // y
                    Constants.PropScreenBounds.x, // width
                    Constants.PropScreenBounds.y // height
                    ),
                Vector2.zero
                );
            _screenBoundariesBehavior.OnBorderReached = OnBorderReached;
        }

        // ========================== Collision ============================

        public Action<Prop> onCollisionEnter = null;
        public Action<Prop> onBorderReached = null;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            onCollisionEnter?.Invoke(this);
        }

        private void OnBorderReached()
        {
            _screenBoundariesBehavior.EnableBorders(false);
            onBorderReached?.Invoke(this);
        }
    }
}