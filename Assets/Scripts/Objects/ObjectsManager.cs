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

    [Header("GameObjects Reference")]
    [ShowIf("activateAndDeactivate")] [SerializeField] private GameObject[] objectsToActivate;
    [ShowIf("activateAndDeactivate")] [SerializeField] private GameObject[] objectsToDeactivate;
    [ShowIf("lockAndUnlock")] [SerializeField] private GameObject[] objectsToLock;
    [ShowIf("lockAndUnlock")] [SerializeField] private GameObject[] objectsToUnlock;

    private void OnEnable()
    {
        if (objectsToActivate.Length > 0 && isOnEnable) StartCoroutine(ActivateAfterTime(delayTime));
        if (objectsToDeactivate.Length > 0 && isOnEnable) StartCoroutine(DeactivateAfterTime(delayTime));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag(PROTAG_TAG)) return;

        if (objectsToActivate.Length > 0 && isTrigger) StartCoroutine(ActivateAfterTime(delayTime));
        if (objectsToDeactivate.Length > 0 && isTrigger) StartCoroutine(DeactivateAfterTime(delayTime));
    }

    // ----------- External Call Functions -----------

    public void ActivateByCalling()
    {
        StartCoroutine(ActivateAfterTime(delayTime));
    }

    public void DeactivateByCalling()
    {
        StartCoroutine(DeactivateAfterTime(delayTime));
    }

    public void DestroyByCalling()
    {
        StartCoroutine(DestroyAfterTime(delayTime));
    }

    // ----------- Coroutine Logic -----------

    IEnumerator ActivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var obj in objectsToActivate) obj.SetActive(true);

        gameObject.SetActive(false);
    }

    IEnumerator DeactivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var obj in objectsToDeactivate) obj.SetActive(false);

        gameObject.SetActive(false);
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var obj in objectsToDeactivate) Destroy(obj);

        gameObject.SetActive(false);
    }
}
