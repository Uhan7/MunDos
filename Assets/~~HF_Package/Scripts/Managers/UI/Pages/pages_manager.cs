using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pages_manager : MonoBehaviour
{
    // Unity GUI stuff ---------------------------------------------------------

    [SerializeField] private int pagesCount;

    [SerializeField] private GameObject[] page;

    [SerializeField] private float[] blankWaitTime;
    [SerializeField] private float[] pageHoldTime;

    public void UpdateArraySizes()
    {
        if (page.Length != pagesCount)
        {
            GameObject[] newPageArray = new GameObject[pagesCount];
            for (int i = 0; i < Mathf.Min(page.Length, pagesCount); i++)
            {
                newPageArray[i] = page[i];
            }
            page = newPageArray;
        }

        if (blankWaitTime.Length != pagesCount)
        {
            float[] newBlankWaitTime = new float[pagesCount];
            for (int i = 0; i < Mathf.Min(blankWaitTime.Length, pagesCount); i++)
            {
                newBlankWaitTime[i] = blankWaitTime[i];
            }
            blankWaitTime = newBlankWaitTime;
        }

        if (pageHoldTime.Length != pagesCount)
        {
            float[] newPageHoldTime = new float[pagesCount];
            for (int i = 0; i < Mathf.Min(pageHoldTime.Length, pagesCount); i++)
            {
                newPageHoldTime[i] = pageHoldTime[i];
            }
            pageHoldTime = newPageHoldTime;
        }
    }

    // Actual code -------------------------------------------------------------

    private void Start()
    {
        StartCoroutine(StartIntro());
    }

    IEnumerator StartIntro()
    {
        for (int i = 0; i < pagesCount; i++)
        {
            yield return new WaitForSeconds(blankWaitTime[i]);
            page[i].SetActive(true);
            if (page[i].transform.Find("Frame") != null) page[i].transform.Find("Frame").gameObject.transform.Find("First Text").gameObject.SetActive(true);
            yield return new WaitForSeconds(pageHoldTime[i] / 4);
            if (page[i].transform.Find("Upper Text") != null) page[i].transform.Find("Upper Text").gameObject.SetActive(true);
            if (page[i].transform.Find("Frame") != null) page[i].transform.Find("Frame").gameObject.transform.Find("Second Text").gameObject.SetActive(true);
            yield return new WaitForSeconds(pageHoldTime[i] / 4);
            if (page[i].transform.Find("Frame") != null)
            {
                page[i].transform.Find("Frame").gameObject.transform.Find("First Text").gameObject.SetActive(false);
                page[i].transform.Find("Frame").gameObject.transform.Find("Second Text").gameObject.SetActive(false);
            }
            if (page[i].transform.Find("Bottom Text") != null) page[i].transform.Find("Bottom Text").gameObject.SetActive(true);
            yield return new WaitForSeconds(pageHoldTime[i] / 4);
            if (page[i].transform.Find("Frame") != null) page[i].transform.Find("Frame").gameObject.transform.Find("Third Text").gameObject.SetActive(true);
            yield return new WaitForSeconds(pageHoldTime[i] / 4);
            page[i].SetActive(false);
            if (i == pagesCount-1) StartGame();
        }
    }

    void StartGame()
    {
        SceneManager.LoadScene("Title Screen");
    }
}
