using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector] private Image imageComponent;

    [Header("ItemData Variables")]
    [HideInInspector] public ItemData data; // Used by InventoryManager.cs

    [Header("Additional Slot Data")]
    [SerializeField] public Sprite defaultSprite;
    [SerializeField] public Sprite selectedSprite;

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
}
