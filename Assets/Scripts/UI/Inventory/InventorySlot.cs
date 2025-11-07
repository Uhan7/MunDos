using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector] private Image imageComponent;
    [SerializeField] private PlayerInteract player;

    [Header("ItemData Variables")]
    [HideInInspector] public ItemData data; // Used by InventoryManager.cs

    [Header("Additional Slot Data")]
    [SerializeField] public Sprite defaultSprite;
    [SerializeField] public Sprite selectedSprite;

    [HideInInspector] public bool isClicked;
    [HideInInspector] public bool isZoomed;
    [SerializeField] public GameObject zoomObject;

    private void Awake()
    {
        if (gameObject.activeInHierarchy) InitializeComponents();
    }

    private void OnEnable()
    {
        if (imageComponent == null) InitializeComponents();
    }

    private void Start()
    {
        if (player == null) Debug.LogError($"Error {this.name}'s PlayerInteract is null");
        if (zoomObject == null) Debug.LogError($"Error {this.name}'s zoomObject is null");
    }

    // Helper Functions --------------------------------------------------------

    private void InitializeComponents()
    {
        imageComponent = GetComponent<Image>();
    }

    public void IsSelected(bool value)
    {
        imageComponent.sprite = value ? selectedSprite : defaultSprite;
    }
    
    public void ToggleStates(int value)
    {
        isClicked = true;
        if (!isZoomed && isClicked)
        {
            //UnityEngine.UI.Image image =  zoomCanvas.transform.GetChild(0).transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Image>();
            zoomObject.GetComponent<UnityEngine.UI.Image>().sprite = this.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite;
            zoomObject.SetActive(true);
            isZoomed = true;
        }
        else if (isZoomed && isClicked)
        {
            zoomObject.SetActive(false);
            isZoomed = false;
        }
        else
        {
            player.SelectItem(value);
        }
            
    }
}
