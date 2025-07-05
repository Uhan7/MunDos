using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // GameObjects
    [SerializeField] private GameObject inventory;

    // Interaction
    [SerializeField] private KeyCode openInventoryKey;
    private bool open;

    void Update()
    {
        AnimationsUpdate();

        InventoryItemsUpdate();

        if (Input.GetKeyDown(openInventoryKey))
        {
            open = !open;
        }
    }

    // Update Functions --------------------------------------------------------

    void AnimationsUpdate()
    {
        inventory.GetComponent<Animator>().SetBool("Open", open);
    }

    void InventoryItemsUpdate()
    {

    }

}
