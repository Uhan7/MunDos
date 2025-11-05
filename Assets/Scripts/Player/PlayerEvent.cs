using UnityEngine;

public class PlayerEvent : MonoBehaviour
{
    //This should call PlayerInteract's PickupItem

    /*
    -Item actualItem gets a nearby object
    -Finds an empty slot and stores it in playerItemIndex
    -itemDatas stores actualItem
    -Moves current item to playerItemIndex

    -actualitem calls PickedUp()
        -Gets InteractableObject component and calls Interact()
            -Itneract activates all interactable components (lock/unlocks, toactivate/todeactivate)
        -Deactivates box collider
        -Destroys iteself
     */

    [Header("References")]
    [SerializeField] private PlayerInteract playerInteract;

    [Header("Properties")]
    [SerializeField] private bool isOnEnable;
    [SerializeField] private bool isTrigger;

    [Header("GameObjects Reference")]
    [SerializeField] private GameObject[] itemsToGive;
    [SerializeField] private GameObject[] itemsToRemove;

    [Header("Flags")]
    [HideInInspector] private bool hasErrors = false;

    private void OnEnable()
    {
        if (isOnEnable)
        {

            GiveItems();
            RemoveItem();
        }
    }
    private void Awake()
    {
        
    }

    private void Start()
    {
        if (!playerInteract)
        {
            Debug.LogError($"Error, {this.name}'s playerInteract is null");
            hasErrors = true;
        }
        if (hasErrors) return;
        InitializeValues();
    }

    public void InitializeValues()
    {

    }

    public void GiveItems()
    {
        if (itemsToGive == null) return;
        foreach (GameObject item in itemsToGive)
        {
            if (item == null) continue;
            playerInteract.GiveItem(item.GetComponent<Item>());
        }
    }

    public void RemoveItem()
    {
        //if (itemsToRemove == null || itemsToRemove.Length == 0) return;
        
        //int size = playerInteract.FindEmptySlot() - 1;
        //Debug.Log($"Slot found at {size}");
        //if (itemsToRemove.Length > size)
        //{
        //    Debug.LogError($"Error, {this.name}'s remove item array greater than inventory count.");
        //    return;
        //}
        //foreach (GameObject item in itemsToRemove)
        //{
        //    if (item == null) continue;
        //    if (playerInteract.TryRemoveItem(item.GetComponent<Item>()))
        //    {

        //    }
        //}
        //foreach(GameObject item in itemsToRemove)
        //{
        //    if (item == null) continue;
        //    Destroy(item.gameObject);
        //    Destroy(item);
        //}
    }


}
