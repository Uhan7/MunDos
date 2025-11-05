using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Unity.VisualScripting;
using System;

public class InteractableObject : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private string PROTAG_TAG = "Protag";
    [HideInInspector] private const string SFX_SOURCE_NAME = "SFX Source";

    [Header("Components")]
    [HideInInspector] public SpriteRenderer spriteRenderer; // Used in PlayerInteract.cs
    [HideInInspector] public Sprite normalSprite; // Used in PlayerInteract.cs
    [SerializeField] public Sprite outlinedSprite; // Used in PlayerInteract.cs

    [Header("References")]
    [HideInInspector] private AudioSource sfxSource;

    [Header("Properties")]
    [SerializeField] public bool itemInteractable; // Used in PlayerInteract.cs
    [SerializeField] private bool checksConditionalObject;
    [SerializeField] private bool unlockInteractableObject;
    [SerializeField] private bool lockInteractableObject;
    [SerializeField] private bool zoomInteract;
    [SerializeField] private bool protagHoverable;
    [SerializeField] private bool willFocus;
    [SerializeField] private bool isTeleporter;

    [Header("Interactions")]
    [SerializeField] private GameObject[] toActivateOnInteract;
    [SerializeField] private GameObject[] toDeactivateOnInteract;

    [Header("Conditionals Interactions")]
    [ShowIf("checksConditionalObject")][SerializeField] private GameObject[] conditionalObjectsToCheck;
    [ShowIf("checksConditionalObject")][SerializeField] private bool checkOnValidInteractOnly;

    [Header("Unlocked Interactions")]
    [ShowIf("unlockInteractableObject")][SerializeField] private GameObject[] objectsToUnlockCheck;
    [ShowIf("unlockInteractableObject")][SerializeField] private bool unlockOnValidInteractOnly;

    [Header("Locked Interactions")]
    [ShowIf("lockInteractableObject")][SerializeField] private GameObject[] objectsToLockCheck;
    [ShowIf("lockInteractableObject")][SerializeField] private bool lockOnValidInteractOnly;

    [Header("Zoom Interactions")]
    [ShowIf("zoomInteract")][SerializeField] private ZoomEnviManager zoomCanvas;

    [Header("On Protag Hover")]
    [ShowIf("protagHoverable")][SerializeField] private bool animateInOnHover;
    [ShowIf("protagHoverable")][SerializeField] private GameObject[] toActivateOnProtagHover;
    [ShowIf("protagHoverable")][SerializeField] private GameObject[] toDeactivateOnProtagHover;

    [Header("Item Interactions")]
    [ShowIf("itemInteractable")][SerializeField] private GameObject[] toActivateOnValidInteract;
    [ShowIf("itemInteractable")][SerializeField] private GameObject[] toDeactivateOnValidInteract;
    [ShowIf("itemInteractable")][SerializeField] private GameObject[] toActivateOnInvalidInteract;
    [ShowIf("itemInteractable")][SerializeField] private GameObject[] toDeactivateOnInvalidInteract;

    [Header("On Interact")]
    [SerializeField] private AudioClip[] soundsToPlay;

    [Header("Flags")]
    [HideInInspector] private bool alreadyCheckedConditional = false;
    [HideInInspector] private bool alreadyCheckedUnlock = false;
    [HideInInspector] private bool alreadyCheckedLock = false;

    private void Awake()
    {
        InitializeCache();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != PROTAG_TAG) return;

        // if (exteriorFG != null) exteriorFG.GetComponent<Animator>().Play("room_fade_in");
        if (animateInOnHover) GetComponent<Animator>().Play("object_fade_in_half");

        SetAll(toActivateOnProtagHover, true);
        SetAll(toDeactivateOnProtagHover, false);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != PROTAG_TAG) return;

        if (animateInOnHover) GetComponent<Animator>().Play("object_fade_out_half");

        SetAll(toActivateOnProtagHover, false);
        SetAll(toDeactivateOnProtagHover, true);
    }

    // Helper Functions --------------------------------------------------------

    private void InitializeCache()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalSprite = spriteRenderer.sprite;

        if (outlinedSprite == null) outlinedSprite = normalSprite;
        sfxSource = GameObject.Find(SFX_SOURCE_NAME).GetComponent<AudioSource>();
    }

    public void Interact()
    {
        foreach (AudioClip clip in soundsToPlay) sfxSource.PlayOneShot(clip);

        SetAll(toActivateOnInteract, true);
        SetAll(toDeactivateOnInteract, false);

        if (zoomInteract) ZoomInteract(true);

        if (conditionalObjectsToCheck != null && !checkOnValidInteractOnly) AddConditionalCheck();
        if (objectsToUnlockCheck != null && !unlockOnValidInteractOnly && unlockInteractableObject) AddUnlockCheck();
        if (objectsToLockCheck != null && !lockOnValidInteractOnly && lockInteractableObject) AddLockCheck();
        if (isTeleporter) GetTeleporter();

        if (willFocus) Focus(true);
    }

    private void GetTeleporter()
    {
        Teleporter teleporter = this.GetComponent<Teleporter>();
        if (teleporter != null)
        {
            teleporter.CanTeleport();
            SetAll(toActivateOnProtagHover, false);
            SetAll(toDeactivateOnProtagHover, true);
        }
        else
        {
        }
    }

    public void ItemInteract(bool var)
    {
        foreach (AudioClip clip in soundsToPlay) sfxSource.PlayOneShot(clip);

        if (!itemInteractable)
        {
            Interact();
            return;
        }

        if (zoomInteract) ZoomInteract(true);

        if (var == false)
        {
            SetAll(toActivateOnInvalidInteract, true);
            SetAll(toDeactivateOnInvalidInteract, false);
        }
        else
        {
            SetAll(toActivateOnValidInteract, true);
            SetAll(toDeactivateOnValidInteract, false);

            if (conditionalObjectsToCheck != null && checkOnValidInteractOnly) AddConditionalCheck();
            if (objectsToUnlockCheck != null && unlockOnValidInteractOnly && unlockInteractableObject) AddUnlockCheck();
            if (objectsToLockCheck != null && lockOnValidInteractOnly && lockInteractableObject) AddLockCheck();
        }

        if (willFocus) Focus(true);
    }

    void SetAll(GameObject[] objects, bool value)
    {
        if (objects == null) return;
        foreach (GameObject obj in objects)
        {
            if (obj == null) continue;
            obj.SetActive(value);
        }
    }

    void AddConditionalCheck()
    {
        if (alreadyCheckedConditional) return;

        foreach (GameObject conditionalObject in conditionalObjectsToCheck)
        {
            ConditionalObject conditionalObjectScript = conditionalObject.GetComponent<ConditionalObject>();

            conditionalObjectScript.currentChecks++;
            if (conditionalObjectScript.currentChecks >= conditionalObjectScript.requiredChecks) conditionalObject.SetActive(true);
        }

        alreadyCheckedConditional = true;
    }

    void AddUnlockCheck()
    {

        if (alreadyCheckedUnlock) return;

        foreach (GameObject lockObject in objectsToUnlockCheck)
        {
            LockableObject lockObjectScript = lockObject.GetComponent<LockableObject>();

            if (lockObjectScript != null)
            {

                CheckUnlock(lockObject, ref lockObjectScript);
                lockObjectScript.currentChecks++;
                if (lockObjectScript.currentChecks >= lockObjectScript.requiredChecks)
                {
                    lockObjectScript.Lock(false);
                }
            }

        }
        alreadyCheckedUnlock = true;
    }
    void AddLockCheck()
    {
        if (alreadyCheckedLock) return;


        foreach (GameObject lockObject in objectsToLockCheck)
        {
            LockableObject lockObjectScript = lockObject.GetComponent<LockableObject>();
            CheckUnlock(lockObject, ref lockObjectScript);


            if (lockObjectScript != null)
            {
                lockObjectScript.currentChecks++;

                if (lockObjectScript.currentChecks >= lockObjectScript.requiredChecks)
                {
                    lockObjectScript.Lock(true);
                }
            }

            alreadyCheckedLock = true;
        }
    }

    void ZoomInteract(bool var)
    {
        if (var == true) zoomCanvas.ActivateZoomedEnvi();
        else zoomCanvas.DeactivateZoomedEnvi();
    }

    void Focus(bool value)
    {
        Parameters param = new Parameters();
        param.PutExtra(ParamNames.IS_FOCUSING_DIALOGUE, value);

        EventBroadcaster.Instance.PostEvent(EventNames.FOCUS_DIALOGUE, param);
    }

    void CheckUnlock(GameObject lockObject, ref LockableObject lockObjectScript)
    {

        if (lockObjectScript == null)
        {
            Transform envi = lockObject.transform.GetChild(0);
            lockObjectScript = envi.GetComponent<LockableObject>();
            if (lockObjectScript == null)
            {
                // Debug.LogError("Error finding LockableObejct of " + lockObject.name);
            }

        }
    }
}
