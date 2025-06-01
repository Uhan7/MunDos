using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data;

    public GameObject objectToInteractWith;

    private void Start()
    {
        data.itemName = gameObject.name;
        data.objectToInteractWith = objectToInteractWith;
    }

    public ItemData GetData()
    {
        return data;
    }
}
