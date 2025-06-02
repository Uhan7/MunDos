using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data;
    public GameObject[] objectsToInteractWith;

    private void Start()
    {
        data.itemName = gameObject.name;
        data.objectsToInteractWith = objectsToInteractWith;
    }

    public ItemData GetData()
    {
        return data;
    }
}
