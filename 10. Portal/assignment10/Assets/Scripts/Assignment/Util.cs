using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace assignment
{
    public class Util
    {
        public static IEnumerator HideTextDelay(Text text, float delay)
        {
            yield return new WaitForSeconds(delay);
            text.enabled = false;
        }
    }
}