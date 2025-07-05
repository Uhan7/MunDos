using UnityEngine;
using UnityEngine.UI;

public class PlayeritemsManager : MonoBehaviour
{
    //[Header("Components")]

    [Header("Protag References")]
    [SerializeField] private GameObject pastProtag;
    [SerializeField] private GameObject presentProtag;

    [Header("Playeritem Slots References")]
    [SerializeField] private GameObject[] pastPlayeritemSlots;
    [SerializeField] private GameObject[] presentPlayeritemSlots;
    [HideInInspector] private PlayerInteract pastProtagScript;
    [HideInInspector] private PlayerInteract presentProtagScript;

    private void Awake()
    {
        InitialCache();
    }

    // THIS IS TEMPORARY - don't put it in Update() since that's too expensive, wait for broadcast manager for dis.
    private void Update()
    {
        DebugsUpdate();

        // TEMP ---
        for (int i = 0; i < pastProtagScript.itemDatas.Length; i++)
        {
            if (pastProtagScript.itemDatas[i].itemName != "") pastPlayeritemSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = pastProtagScript.itemDatas[i].itemSprite;
        }
        // TEMP ---
    }

    // Helper Functions --------------------------------------------------------

    void DebugsUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Equals)) Debug.Log(pastProtag.GetComponent<PlayerInteract>().currentItemData.itemName);
    }

    void InitialCache()
    {
        pastProtagScript = pastProtag.GetComponent<PlayerInteract>();
        presentProtagScript = presentProtag.GetComponent<PlayerInteract>();
    }
}
