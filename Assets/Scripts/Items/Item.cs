using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data;

    public GameObject[] objectToInteractWith;

    private void Start()
    {
        data.itemName = gameObject.name;
        foreach (GameObject obj in objectToInteractWith)
            data.objectToInteractWith = obj;
    }

    public ItemData GetData()
    {
        return data;
    }
}
