using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject InventorySlots;
    [SerializeField] private KeyCode openInventoryKey;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Update()
    {
        if (Input.GetKeyDown(openInventoryKey))
        {
            InventorySlots.SetActive(!InventorySlots.activeInHierarchy);
        }
    }

    public void ToggleInventorySlots()
    {
        InventorySlots.SetActive(!InventorySlots.activeInHierarchy);
    }
}
