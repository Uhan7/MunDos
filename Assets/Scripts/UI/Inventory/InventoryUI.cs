using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] GameObject gameMan;
    [SerializeField] GameObject InventorySlots;
    [SerializeField] private KeyCode openInventoryKey;
    [SerializeField] Animator invButtonAnimator;

    [HideInInspector] private Animator animator;
    [HideInInspector] private GameManager gameManager;
    [HideInInspector] public bool WillUpdate;

    [SerializeField] private AudioClip inventoryOpenSFX;
    [SerializeField] private AudioClip inventoryCloseSFX;

    private void Start()
    {
        InitializeValues();
        animator = GetComponent<Animator>();
        gameManager = gameMan.GetComponent<GameManager>();
        WillUpdate = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(openInventoryKey) && !gameManager.isHidingUI && !gameManager.isPaused)
        {
            ToggleInventorySlots();
        }
    }

    // Helper Functions --------------------------------------------------------

    public void ToggleInventorySlots()
    {
        WillUpdate = true;
        animator.SetBool("isOpen", !animator.GetBool("isOpen"));
        if (sfxSource != null) sfxSource.PlayOneShot(animator.GetBool("isOpen") ? inventoryOpenSFX : inventoryCloseSFX);
    }

    void InitializeValues()
    {
        InventorySlots.SetActive(false);
    }

    public void SetIsOpenFalse()
    {
        animator.SetBool("isOpen", false);
    }

    public void FlashButton()
    {
        invButtonAnimator.SetTrigger("Flash");
    }
}
