using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data;

    private void Start()
    {
        if (data.itemName == "") data.itemName = gameObject.name;
    }

    public ItemData GetData()
    {
        return data;
    }

    public void PickedUp()
    {
        GetComponent<InteractableObject>().Interact();

        // Put anims here that will make it fade out or whatever

        Destroy(gameObject, 0.5f);
    }
}
