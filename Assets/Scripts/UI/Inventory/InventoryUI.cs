using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject InventorySlots;
    [SerializeField] private KeyCode openInventoryKey;

    private void Start()
    {
        InitializeValues();
    }

    private void Update()
    {
        if (Input.GetKeyDown(openInventoryKey))
        {
            InventorySlots.SetActive(!InventorySlots.activeInHierarchy);
        }
    }

    // Helper Functions --------------------------------------------------------

    public void ToggleInventorySlots()
    {
        InventorySlots.SetActive(!InventorySlots.activeInHierarchy);
    }

    void InitializeValues()
    {
        InventorySlots.SetActive(false);
    }
}
