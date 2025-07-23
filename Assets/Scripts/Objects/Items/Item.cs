using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Data Variables")]
    [SerializeField] public ItemData data;
    [SerializeField] private bool hasDialogue;

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
        GetComponent<BoxCollider2D>().enabled = false;

        if (hasDialogue) GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        else Destroy(gameObject);
    }
}
