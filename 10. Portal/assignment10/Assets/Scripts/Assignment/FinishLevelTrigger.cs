using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinishLevelTrigger : MonoBehaviour
{
    [SerializeField] private Text finishText;
    [SerializeField] [Range(0f, 10f)] private float finishScreenTime = 3;

    private const string PLAYER_TAG = "Player";

    private void Awake()
    {
        if (finishText.isActiveAndEnabled)
            finishText.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == PLAYER_TAG)
        {
            finishText.enabled = true;
            StartCoroutine(Util.HideTextDelay(finishText, finishScreenTime));
        }
    }
}
