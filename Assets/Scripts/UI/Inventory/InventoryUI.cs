using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject InvSlots;
    [SerializeField] private KeyCode openInventoryKey;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Update()
    {
        if (Input.GetKeyDown(openInventoryKey))
        {
            InvSlots.SetActive(!InvSlots.activeInHierarchy);
        }
    }

    public void toggleInventorySlots()
    {
        InvSlots.SetActive(!InvSlots.activeInHierarchy);
    }
}
