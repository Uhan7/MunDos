using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector] private Image imageComponent;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerInteract player;
    [SerializeField] public GameObject zoomObject;

    [Header("ItemData Variables")]
    [HideInInspector] public ItemData data; // Used by InventoryManager.cs

    [Header("Additional Slot Data")]
    [SerializeField] public Sprite defaultSprite;
    [SerializeField] public Sprite selectedSprite;

    [Header("Flags")]
    [HideInInspector] public bool isClicked;
    [HideInInspector] public bool isZoomed;
    

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
        if (gameManager == null) Debug.LogError($"Error {this.name}'s gameManager is null");
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
        if (!isZoomed && !isClicked)
        {
            Debug.Log($"isClicked {isClicked}");
            isClicked = true;
        }
        else if (!isZoomed && isClicked)
        {
            Debug.Log($"1 isZoomed {isZoomed}, isClicked {isClicked}");
            //UnityEngine.UI.Image image =  zoomCanvas.transform.GetChild(0).transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Image>();
            zoomObject.GetComponent<UnityEngine.UI.Image>().sprite = this.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite;
            SetZoom(true);
        }
        else if (isZoomed && isClicked)
        {
            Debug.Log($"2 isZoomed {isZoomed}, isClicked {isClicked}");
            SetZoom(false);
        }
        else
        {
            Debug.Log($"3 isZoomed {isZoomed}, isClicked {isClicked}");
            player.SelectValidItem(value);
        }
            
    }
    public void SetZoom(bool value)
    {
        Focus(value);
        zoomObject.SetActive(value);
        isZoomed = value;
    }
    void Focus(bool value)
    {
        Parameters param = new Parameters();
        param.PutExtra(ParamNames.IS_FOCUSING_DIALOGUE, value);

        EventBroadcaster.Instance.PostEvent(EventNames.FOCUS_DIALOGUE, param);
    }
}
