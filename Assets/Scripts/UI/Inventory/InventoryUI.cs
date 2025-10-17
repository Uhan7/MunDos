using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject gameMan;
    [SerializeField] GameObject InventorySlots;
    [SerializeField] private KeyCode openInventoryKey;

    private Animator animator;
    private GameManager gameManager;

    private void Start()
    {
        InitializeValues();
        animator = GetComponent<Animator>();
        gameManager = gameMan.GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(openInventoryKey) && !gameManager.isHidingUI)
        {
            ToggleInventorySlots();
        }
    }

    // Helper Functions --------------------------------------------------------

    public void ToggleInventorySlots()
    {
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
