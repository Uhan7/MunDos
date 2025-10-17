using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject InventorySlots;
    [SerializeField] private KeyCode openInventoryKey;

    private Animator animator;

    private void Start()
    {
        InitializeValues();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(openInventoryKey))
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
}
