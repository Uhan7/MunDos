using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class InteractableObject : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private string PROTAG_TAG = "Protag";
    [HideInInspector] private string ZOOMED_ENVI_BACKDROP_NAME = "Zoomed Envi Backdrop";
    [HideInInspector] private string ZOOMED_ENVI_IMAGE_NAME = "Zoomed Envi Image";

    [Header("Components")]
    [HideInInspector] public SpriteRenderer spriteRenderer; // Used in PlayerInteract.cs
    [HideInInspector] public Sprite normalSprite; // Used in PlayerInteract.cs
    [SerializeField] public Sprite outlinedSprite; // Used in PlayerInteract.cs

    [Header("Properties")]
    [SerializeField] public bool itemInteractable; // Used in PlayerInteract.cs
    [SerializeField] private bool checksConditionalObject;
    [SerializeField] private bool unlockInteractableObject;
    [SerializeField] private bool lockInteractableObject;
    [SerializeField] private bool zoomInteract;
    [SerializeField] private bool puzzleInteract;
    [SerializeField] private bool protagHoverable;

    [Header("Interactions")]
    [SerializeField] private GameObject[] toActivateOnInteract;
    [SerializeField] private GameObject[] toDeactivateOnInteract;

    [Header("Conditionals Interactions")]
    [ShowIf("checksConditionalObject")] [SerializeField] private GameObject[] conditionalObjectsToCheck;
    [ShowIf("checksConditionalObject")] [SerializeField] private bool checkOnValidInteractOnly;

    [Header("Unlocked Interactions")]
    [ShowIf("unlockInteractableObject")] [SerializeField] private GameObject[] objectsToUnlockCheck;
    [ShowIf("unlockInteractableObject")] [SerializeField] private bool unlockOnValidInteractOnly;

    [Header("Locked Interactions")]
    [ShowIf("lockInteractableObject")] [SerializeField] private GameObject[] objectsToLockCheck;
    [ShowIf("lockInteractableObject")] [SerializeField] private bool lockOnValidInteractOnly;

    [Header("Zoom Interactions")]
    [ShowIf("zoomInteract")] [SerializeField] private Sprite zoomedEnviSprite;

    [Header("Puzzle Interactions")]
    [ShowIf("puzzleInteract")] [SerializeField] private GameObject puzzleObject;

    [Header("On Protag Hover")]
    [ShowIf("protagHoverable")] [SerializeField] private GameObject exteriorFG;
    [ShowIf("protagHoverable")] [SerializeField] private bool animateInOnHover;
    [ShowIf("protagHoverable")] [SerializeField] private GameObject[] toActivateOnProtagHover;
    [ShowIf("protagHoverable")] [SerializeField] private GameObject[] toDeactivateOnProtagHover;

    [Header("Item Interactions")]
    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toActivateOnValidInteract;
    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toDeactivateOnValidInteract;
    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toActivateOnInvalidInteract;
    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toDeactivateOnInvalidInteract;

    [Header("Flags")]
    [HideInInspector] private bool alreadyCheckedConditional = false;
    [HideInInspector] private bool alreadyCheckedUnlock = false;
    [HideInInspector] private bool alreadyCheckedLock = false;
    [HideInInspector] private bool zoomingOnEnvi = false;

    private void Awake()
    {
        InitializeCache();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != PROTAG_TAG) return;

        if (exteriorFG != null) exteriorFG.GetComponent<Animator>().Play("room_fade_in");
        if (animateInOnHover) GetComponent<Animator>().Play("object_fade_in_half");

        SetAll(toActivateOnProtagHover, true);
        SetAll(toDeactivateOnProtagHover, false);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != PROTAG_TAG) return;

        if (exteriorFG != null) exteriorFG.GetComponent<Animator>().Play("room_fade_out");
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
    }

    public void Interact()
    {
        SetAll(toActivateOnInteract, true);
        SetAll(toDeactivateOnInteract, false);

        if (conditionalObjectsToCheck != null && !checkOnValidInteractOnly) AddConditionalCheck();
        if (objectsToUnlockCheck != null && !unlockOnValidInteractOnly) AddUnlockCheck();
        if (objectsToLockCheck != null && !lockOnValidInteractOnly) AddLockCheck();

        if (zoomInteract) ZoomInteract(zoomingOnEnvi);
    }

    public void ItemInteract(bool var)
    {
        if (!itemInteractable)
        {
            Interact();
            return;
        }

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
            if (objectsToUnlockCheck != null && unlockOnValidInteractOnly) AddUnlockCheck();
            if (objectsToLockCheck != null && lockOnValidInteractOnly) AddLockCheck();
        }

        if (zoomInteract) ZoomInteract(zoomingOnEnvi);
    }

    void SetAll(GameObject[] objects, bool value)
    {
        if (objects == null) return;
        foreach (GameObject obj in objects) obj.SetActive(value);
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

            lockObjectScript.currentChecks++;
            if (lockObjectScript.currentChecks >= lockObjectScript.requiredChecks) lockObjectScript.Lock(false);
        }

        alreadyCheckedUnlock = true;
    }

    void AddLockCheck()
    {
        if (alreadyCheckedLock) return;

        foreach (GameObject lockObject in objectsToLockCheck)
        {
            LockableObject lockObjectScript = lockObject.GetComponent<LockableObject>();

            lockObjectScript.currentChecks++;
            if (lockObjectScript.currentChecks >= lockObjectScript.requiredChecks) lockObjectScript.Lock(true);
        }

        alreadyCheckedLock = true;
    }

    void ZoomInteract(bool value)
    {
        Parameters param = new Parameters();
        param.PutExtra(ParamNames.IS_ZOOMING_ENVI, !value);
        param.PutExtra(ParamNames.ZOOM_ENVI_SPRITE, zoomedEnviSprite);

        EventBroadcaster.Instance.PostEvent(EventNames.ZOOM_ENVI, param);
    }
}
