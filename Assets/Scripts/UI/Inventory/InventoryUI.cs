using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject gameMan;
    [SerializeField] GameObject InventorySlots;
    [SerializeField] private KeyCode openInventoryKey;

    [HideInInspector] private Animator animator;
    [HideInInspector] private GameManager gameManager;
    [HideInInspector] public bool WillUpdate;

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
        InventorySlots.SetActive(!InventorySlots.activeInHierarchy);
    }

    void InitializeValues()
    {
        InventorySlots.SetActive(false);
    }

    public void SetIsOpenFalse()
    {
        animator.SetBool("isOpen", false);
    }
}
