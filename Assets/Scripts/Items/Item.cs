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
}
