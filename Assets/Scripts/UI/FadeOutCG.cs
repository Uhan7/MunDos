using UnityEngine;
using System.Collections;

public class FadeOutCG : MonoBehaviour
{
    private void OnDisable()
    {
        // StartCoroutine(FadeOut(1f));
    }

    private IEnumerator FadeOut(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
