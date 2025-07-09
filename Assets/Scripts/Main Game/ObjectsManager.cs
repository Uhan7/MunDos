using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : MonoBehaviour
{
    public GameObject[] toActivate;
    public GameObject[] toDeactivate;

    public bool activateOnTrigger;
    public bool activateOnCollision;
    public bool activateOnEnable;

    public bool deactivateOnTrigger;
    public bool deactivateOnCollision;
    public bool deactivateOnEnable;

    public float waitTime;

    public bool deactivateThisObjectAfter;
    public bool causeScreenShake;

    private void OnEnable()
    {
        if (activateOnEnable)
            StartCoroutine(ActivateAfterTime(waitTime));

        if (deactivateOnEnable)
            StartCoroutine(DeactivateAfterTime(waitTime));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (activateOnTrigger)
                StartCoroutine(ActivateAfterTime(waitTime));

            if (deactivateOnTrigger)
                StartCoroutine(DeactivateAfterTime(waitTime));
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (activateOnCollision)
                StartCoroutine(ActivateAfterTime(waitTime));

            if (deactivateOnCollision)
                StartCoroutine(DeactivateAfterTime(waitTime));
        }
    }

    // ----------- External Call Functions -----------

    public void ActivateByCalling()
    {
        StartCoroutine(ActivateAfterTime(waitTime));
    }

    public void DeactivateByCalling()
    {
        StartCoroutine(DeactivateAfterTime(waitTime));
    }

    public void DestroyByCalling()
    {
        StartCoroutine(DestroyAfterTime(waitTime));
    }

    public void DeactivateSelfByCalling()
    {
        StartCoroutine(DeactivateSelfAfterTime(waitTime));
    }

    // ----------- Coroutine Logic -----------

    IEnumerator ActivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var obj in toActivate)
            obj.SetActive(true);

        if (deactivateThisObjectAfter)
            gameObject.SetActive(false);

        //if (causeScreenShake) StartCoroutine(gameObject.GetComponent<camera_shake>().screenShake());
    }

    IEnumerator DeactivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var obj in toDeactivate)
            obj.SetActive(false);

        //if (causeScreenShake) StartCoroutine(gameObject.GetComponent<camera_shake>().screenShake());
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var obj in toDeactivate)
            Destroy(obj);

        //if (causeScreenShake) StartCoroutine(gameObject.GetComponent<camera_shake>().screenShake());
    }

    IEnumerator DeactivateSelfAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
