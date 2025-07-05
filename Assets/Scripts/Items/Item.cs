using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Data Variables")]
    [SerializeField] public ItemData data;

    private void Start()
    {
        if (data.itemName == "") data.itemName = gameObject.name;
        if (data.itemSprite == null) data.itemSprite = GetComponent<SpriteRenderer>().sprite;
    }

    public ItemData GetData()
    {
        ItemData copy = new ItemData(data);

        if (copy.itemName == "") copy.itemName = gameObject.name;
        if (copy.itemSprite == null) copy.itemSprite = GetComponent<SpriteRenderer>().sprite;

        return copy;
    }

    public void PickedUp()
    {
        GetComponent<InteractableObject>().Interact();

        // Put anims here that will make it fade out or whatever

        Destroy(gameObject, 0.5f);
    }
}
