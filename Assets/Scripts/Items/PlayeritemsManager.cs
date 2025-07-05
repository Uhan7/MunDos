using UnityEngine;

public class PlayeritemsManager : MonoBehaviour
{
    //[Header("Components")]

    [Header("Protag References")]
    [SerializeField] private GameObject pastProtag;
    [SerializeField] private GameObject presentProtag;

    [Header("Playeritem Slots References")]
    [SerializeField] private GameObject[] pastPlayeritemSlots;
    [SerializeField] private GameObject[] presentPlayeritemSlots;

    // THIS IS TEMPORARY - don't put it in Update() since that's too expensive, wait for broadcast manager for dis.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) Debug.Log(pastProtag.GetComponent<PlayerInteract>().currentItemData.itemName);
    }
}
