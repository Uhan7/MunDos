using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineManager : MonoBehaviour
{
    [Header("Key Inputs")]
    [SerializeField] private KeyCode switchTimelineKey;

    [Header("References")]
    [SerializeField] private GameObject pastTimeline;
    [SerializeField] private GameObject presentTimeline;
    [SerializeField] private GameObject presentTransition;
    [SerializeField] private GameObject pastTransition;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip transitionSound;

    [Header("Flags")]
    [HideInInspector] private int currentTimeline; // 0 is Past | 1 is Present
    [HideInInspector] public bool canSwitch = true; // Used in GameManager.cs

    void Start()
    {
        if (pastTimeline.activeInHierarchy) currentTimeline = 0;
        else currentTimeline = 1;
    }

    void Update()
    {
        if (!canSwitch) return;

        if (Input.GetKeyDown(switchTimelineKey)) StartCoroutine(SwitchTimeline(0.05f));
    }

    // Update Functions --------------------------------------------------------

    IEnumerator SwitchTimeline(float delayTime)
    {
        // Delay
        yield return new WaitForSeconds(delayTime);

        // modify this area to have cooler effects and stuff


        if (currentTimeline == 0)
        {
            pastTimeline.SetActive(false);
            presentTimeline.SetActive(true);

            currentTimeline = 1;
        }
        else
        {
            pastTimeline.SetActive(true);
            presentTimeline.SetActive(false);

            currentTimeline = 0;
        }

        TimelineTransition();
    }

    void TimelineTransition()
    {
        if (currentTimeline == 0)
        {
            pastTransition.SetActive(true);
            presentTransition.SetActive(false);
            audioSource.PlayOneShot(transitionSound);
        }
        else
        {
            pastTransition.SetActive(false);
            presentTransition.SetActive(true);
            audioSource.PlayOneShot(transitionSound);
        }
    }
}
