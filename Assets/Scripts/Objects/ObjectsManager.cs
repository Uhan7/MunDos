using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ObjectsManager : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const string PROTAG_TAG = "Protag";

    [Header("Properties")]
    [SerializeField] private bool isOnEnable;
    [SerializeField] private bool isTrigger;
    [SerializeField] private bool activateAndDeactivate;
    [SerializeField] private bool lockAndUnlock;
    [SerializeField] private float delayTime;
    [SerializeField] private bool resetAfter;
    [SerializeField] private bool deactivateAfter;

    [Header("GameObjects Reference")]
    [ShowIf("activateAndDeactivate")] [SerializeField] private GameObject[] objectsToActivate;
    [ShowIf("activateAndDeactivate")] [SerializeField] private GameObject[] objectsToDeactivate;
    [ShowIf("lockAndUnlock")] [SerializeField] private GameObject[] objectsToLock;
    [ShowIf("lockAndUnlock")] [SerializeField] private GameObject[] objectsToUnlock;

    [Header("Flags")]
    [HideInInspector] private bool alreadyCheckedUnlock = false;
    [HideInInspector] private bool alreadyCheckedLock = false;


    private void OnEnable()
    {
        if (activateAndDeactivate && isOnEnable)
        {
            if (objectsToActivate.Length > 0) StartCoroutine(ActivateAfterTime(delayTime));
            if (objectsToDeactivate.Length > 0) StartCoroutine(DeactivateAfterTime(delayTime));
        }
        if (lockAndUnlock && isOnEnable)
        {
            if (objectsToLock.Length > 0) StartCoroutine(LockAfterTime(delayTime));
            if (objectsToLock.Length > 0) StartCoroutine(LockAfterTime(delayTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag(PROTAG_TAG)) return;

        if (activateAndDeactivate && isTrigger)
        {
            if (objectsToActivate.Length > 0) StartCoroutine(ActivateAfterTime(delayTime));
            if (objectsToDeactivate.Length > 0) StartCoroutine(DeactivateAfterTime(delayTime));
        }
        if (lockAndUnlock && isTrigger)
        {
            if (objectsToLock.Length > 0) StartCoroutine(LockAfterTime(delayTime));
            if (objectsToUnlock.Length > 0) StartCoroutine(UnlockAfterTime(delayTime));
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag(PROTAG_TAG)) return;

        print("exited");
        if (resetAfter) ResetValues();
        if (deactivateAfter) Deactivate();
    }

    // ----------- External Call Functions -----------

    public void ActivateByCalling()
    {
        StartCoroutine(ActivateAfterTime(delayTime));
    }

    public void DeactivateByCalling()
    {
        StartCoroutine(DeactivateAfterTime(delayTime + 0.02f));
    }

    public void DestroyByCalling()
    {
        StartCoroutine(DestroyAfterTime(delayTime + 0.04f));
    }

    // ----------- Coroutine Logic -----------

    IEnumerator ActivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var obj in objectsToActivate) obj.SetActive(true);
    }

    IEnumerator DeactivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var obj in objectsToDeactivate) obj.SetActive(false);
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var obj in objectsToDeactivate) Destroy(obj);

        gameObject.SetActive(false);
    }

    IEnumerator LockAfterTime(float time)
    {
        if (alreadyCheckedLock)
        {
            yield break;
        }

        yield return new WaitForSeconds(time);
        foreach (var obj in objectsToLock)
        {
            LockableObject lockObjectScript = obj.GetComponent<LockableObject>();
            if (lockObjectScript == null)
            {
                continue;
            }

            CheckUnlock(obj, ref lockObjectScript);
            lockObjectScript.currentChecks++;
            if (lockObjectScript.currentChecks >= lockObjectScript.requiredChecks)
            {
                lockObjectScript.Lock(true);
            }
        }
        alreadyCheckedLock = true;
    }

    IEnumerator UnlockAfterTime(float time)
    {
        if (alreadyCheckedUnlock)
        {
            yield break;
        }

        yield return new WaitForSeconds(time);
        foreach (var obj in objectsToUnlock)
        {
            LockableObject lockObjectScript = obj.GetComponent<LockableObject>();
            if (lockObjectScript == null)
            {
                continue;
            }

            CheckUnlock(obj, ref lockObjectScript);
            lockObjectScript.currentChecks++;
            if (lockObjectScript.currentChecks >= lockObjectScript.requiredChecks)
            {
                lockObjectScript.Lock(false);
            }
        }
        alreadyCheckedUnlock = true;
    }

    // ----------- Helper Functions -----------
    void CheckUnlock(GameObject lockObject, ref LockableObject lockObjectScript)
    {
        if (lockObjectScript != null)
        {
            return;
        }

        Transform envi = lockObject.transform.GetChild(0);
        if (envi != null)
        {
            lockObjectScript = envi.GetComponent<LockableObject>();
            if (lockObjectScript == null)
            {
                Debug.LogError("Error finding LockableObejct of " + lockObject.name);
            }
        }
        else
        {
            Debug.LogError("Error, no child found for " + lockObject.name);
        }
    }

    void ResetValues()
    {
        alreadyCheckedLock = false;
        alreadyCheckedUnlock = false;
        foreach (var obj in objectsToLock)
        {
            LockableObject lockObjectScript = obj.GetComponent<LockableObject>();
            if (lockObjectScript != null)
            {
                lockObjectScript.currentChecks = 0;
            }
        }
        foreach (var obj in objectsToUnlock)
        {
            LockableObject lockObjectScript = obj.GetComponent<LockableObject>();
            if (lockObjectScript != null)
            {
                lockObjectScript.currentChecks = 0;
            }
        }
    }
    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
