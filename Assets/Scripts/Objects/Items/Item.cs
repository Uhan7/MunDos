using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UniqueID))]
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
        //Debug.Log($"Item>Getting Data name {copy.itemName}");

        Debug.Log($"{gameObject.name} | ItemData instance ID: {data.GetHashCode()}");

        return copy;
    }

    public void PickedUp()
    {
        if (!gameObject.activeSelf) return;

        GetComponent<InteractableObject>().Interact();
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        StartCoroutine(DeactivateSelf(0.5f));
    }

    IEnumerator DeactivateSelf(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
