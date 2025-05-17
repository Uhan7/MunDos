using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // GameObjects
    [SerializeField] private GameObject inventory;

    // Interaction
    [SerializeField] private KeyCode openInventoryKey;
    private bool open;

    // Actual Items
    //[SerializeField] private GameObject[] items;
    // Try using GridCanvas or smth to organize... find that shit out...

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
