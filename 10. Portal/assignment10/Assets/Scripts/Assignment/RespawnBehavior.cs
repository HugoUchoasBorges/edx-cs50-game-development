using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RespawnBehavior : MonoBehaviour
{
    [SerializeField] private Transform respawnPosition;
    [SerializeField] private Text respawnText;
    [SerializeField] [Range(0f, 3f)] private float respawnScreenTime = 1;
    [SerializeField] [Range(-100f, -1.5f)] private float respawnHeight = -15;

    private void Awake()
    {
        if (respawnText.isActiveAndEnabled)
            respawnText.enabled = false;
    }

    private void Update()
    {
        if (transform.position.y < respawnHeight)
        {
            respawnText.enabled = true;
            transform.position = respawnPosition.position;
            StartCoroutine(Util.HideTextDelay(respawnText, respawnScreenTime));
        }
    }
}
