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

    private void Awake()
    {
        InitializeCache();
    }

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

    private void InitializeCache()
    {

    }

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


//    void AddConditionalCheck()
//    {
//        if (alreadyCheckedConditional) return;

//        foreach (GameObject conditionalObject in conditionalObjectsToCheck)
//        {
//            ConditionalObject conditionalObjectScript = conditionalObject.GetComponent<ConditionalObject>();

//            conditionalObjectScript.currentChecks++;
//            if (conditionalObjectScript.currentChecks >= conditionalObjectScript.requiredChecks) conditionalObject.SetActive(true);
//        }

//        alreadyCheckedConditional = true;
//    }

//    void AddUnlockCheck()
//    {

//        if (alreadyCheckedUnlock)
//        {
//            return;
//        }

//        foreach (GameObject lockObject in objectsToUnlockCheck)
//        {
//            LockableObject lockObjectScript = lockObject.GetComponent<LockableObject>();

//            if (lockObjectScript != null)
//            {

//                CheckUnlock(lockObject, ref lockObjectScript);
//                lockObjectScript.currentChecks++;
//                if (lockObjectScript.currentChecks >= lockObjectScript.requiredChecks)
//                {
//                    lockObjectScript.Lock(false);
//                }
//            }

//        }
//        alreadyCheckedUnlock = true;
//    }
//    void AddLockCheck()
//    {
//        if (alreadyCheckedLock) return;


//        foreach (GameObject lockObject in objectsToLockCheck)
//        {
//            LockableObject lockObjectScript = lockObject.GetComponent<LockableObject>();
//            CheckUnlock(lockObject, ref lockObjectScript);


//            if (lockObjectScript != null)
//            {
//                lockObjectScript.currentChecks++;

//                if (lockObjectScript.currentChecks >= lockObjectScript.requiredChecks)
//                {
//                    lockObjectScript.Lock(true);
//                }
//            }

//            alreadyCheckedLock = true;
//        }
//    }

//    void ZoomInteract(bool var)
//    {
//        if (var == true) zoomCanvas.ActivateZoomedEnvi();
//        else zoomCanvas.DeactivateZoomedEnvi();
//    }

//    void Focus(bool value)
//    {
//        Parameters param = new Parameters();
//        param.PutExtra(ParamNames.IS_FOCUSING_DIALOGUE, value);

//        EventBroadcaster.Instance.PostEvent(EventNames.FOCUS_DIALOGUE, param);
//    }

//    void CheckUnlock(GameObject lockObject, ref LockableObject lockObjectScript)
//    {

//        if (lockObjectScript == null)
//        {
//            Transform envi = lockObject.transform.GetChild(0);
//            lockObjectScript = envi.GetComponent<LockableObject>();
//            if (lockObjectScript == null)
//            {
//                // Debug.LogError("Error finding LockableObejct of " + lockObject.name);
//            }

//        }
//        else
//        {
//        }
//    }
//}
