using UnityEngine;
using UnityEngine.UI;

namespace assignment
{
    public class TextTrigger : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] [Range(0f, 10f)] private float screenTime = 3;

        private const string PLAYER_TAG = "Player";

        private void Awake()
        {
            if (text.isActiveAndEnabled)
                text.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == PLAYER_TAG)
            {
                text.enabled = true;
                StartCoroutine(Util.HideTextDelay(text, screenTime));
            }
        }
    }
}