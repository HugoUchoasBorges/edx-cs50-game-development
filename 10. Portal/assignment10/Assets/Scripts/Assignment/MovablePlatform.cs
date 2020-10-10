using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace assignment
{
    public class MovablePlatform : MonoBehaviour
    {
        [SerializeField] private Transform[] positions;
        [SerializeField] [Range(1f, 10f)] private float speed = 1f;
        private int currPosition = 0;

        private void Start()
        {
            StartCoroutine(move());
        }

        private IEnumerator move()
        {
            Vector3 from = positions[currPosition].position;
            int nextPosition = currPosition + 1 >= positions.Length ? 0 : currPosition + 1;
            Vector3 to = positions[nextPosition].position;

            while (true)
            {
                from = positions[currPosition].position;
                nextPosition = currPosition + 1 >= positions.Length ? 0 : currPosition + 1;
                to = positions[nextPosition].position;

                float step = (speed / (from - to).magnitude) * Time.fixedDeltaTime;
                float t = 0;
                while (t <= 1.0f)
                {
                    t += step; // Goes from 0 to 1, incrementing by step each time
                    transform.position = Vector3.Lerp(from, to, t); // Move objectToMove closer to b
                    yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
                }
                transform.position = to;
                currPosition = nextPosition;

            }
        }
    }
}
