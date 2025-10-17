using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineManager : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const string DIALOGUE_HOLDER_NAME = "Dialogue Holder";
    [HideInInspector] private const string CURRENT_PROTAG_TAG = "Protag";

    [Header("References")]
    [HideInInspector] private DialogueManager dialogueHolder;

    [Header("Key Inputs")]
    [SerializeField] private KeyCode switchTimelineKey;
    [SerializeField] private KeyCode interactKey;

    [Header("Timelines")]
    [SerializeField] private GameObject pastTimeline;
    [SerializeField] private GameObject presentTimeline;

    [Header("Visuals")]
    [SerializeField] private GameObject presentTransition;
    [SerializeField] private GameObject pastTransition;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip transitionSound;

    [Header("Flags")]
    [HideInInspector] private int currentTimeline; // 0 is Past | 1 is Present
    [HideInInspector] public bool canSwitch = true; // Used in GameManager.cs
    [HideInInspector] public bool timelineUnlocked = false;

    void Start()
    {
        if (pastTimeline.activeInHierarchy) currentTimeline = 0;
        else currentTimeline = 1;
    }

    void Update()
    {
        if (!canSwitch || !timelineUnlocked) return;

        if (Input.GetKeyDown(switchTimelineKey) && !Input.GetKey(interactKey))
        {
            PlayerMove playerMoveScript = GameObject.FindGameObjectWithTag(CURRENT_PROTAG_TAG).GetComponent<PlayerMove>();
            if (!playerMoveScript.canInput) return;

            StartCoroutine(SwitchTimeline(0.00f));
        }
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
