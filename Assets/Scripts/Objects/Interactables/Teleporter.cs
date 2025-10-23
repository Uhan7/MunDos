using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Unity.VisualScripting;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private string PROTAG_TAG = "Protag";

    [Header("Components")]
    //Gameobject endpoint
    //This gameobject as startpoint
    [SerializeField] private GameObject teleportTo;
    [HideInInspector] public Transform player;

    [Header("Properties")]
    [SerializeField] private bool startFromTrigger;
    [SerializeField] private bool deactivateHereAfter;
    [SerializeField] private bool deactivateThereAfter;
    [SerializeField] private bool hasDelay;
    [SerializeField] private bool hasFade;

    [Header("Delay")]
    [ShowIf("hasDelay")][SerializeField] private float delayDurationBeforeTeleport = 0.5f;

    [Header("Fade")]
    [ShowIf("hasFade")][SerializeField] private float fadeAnimDuration = 0.5f;
    [ShowIf("hasFade")][SerializeField] private float fadeHoldDuration = 0.2f;
    [ShowIf("hasFade")][SerializeField] public Image fadeImage;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != PROTAG_TAG) return;
        if (col.gameObject.tag == PROTAG_TAG) player = col.gameObject.transform;

        if (startFromTrigger)
        {
            CanTeleport();
        }
        
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != PROTAG_TAG) return;
        DeactivateAfter();

    }

    // Helper Functions --------------------------------------------------------

    public void CanTeleport()
    {
        if (hasDelay) {
            StartCoroutine(TeleportWithDelay());
        }
        else
        {
            PerformTeleport();
        }
    }

    private void PerformTeleport()
    {
        if (teleportTo != null)
        {
            player.position = teleportTo.transform.position;
        }
        else
        {
            Debug.LogError("Error TeleportTo " + teleportTo.name + " is null");
        }
        DeactivateAfter();
    }

    private IEnumerator TeleportWithDelay()
    {
        yield return new WaitForSeconds(delayDurationBeforeTeleport);
        PerformTeleport();
        //if (hasFade) yield return StartCoroutine(FadeScreen());
    }

    /*
    private IEnumerator FadeScreen()
    {
        float startAlpha = fadeImage.color.a;
        float timeElapsed = 0f;
        float targetAlpha = 0f;

        while (timeElapsed < fadeAnimDuration)
        {
            float currentAlpa = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / fadeAnimDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, currentAlpa);
            yield return null;
        }
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);
    }
    */

    public void DeactivateAfter()
    {
        if (deactivateHereAfter)
        {
            gameObject.SetActive(false);
        }
        if (deactivateThereAfter)
        {
            teleportTo.SetActive(false);
        }
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
}