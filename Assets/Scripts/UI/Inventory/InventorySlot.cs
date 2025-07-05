using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector] private Image imageComponent;

    [Header("ItemData Variables")]
    [SerializeField] public ItemData data; // Used by InventoryManager.cs

    [Header("Additional Slot Data")]
    [SerializeField] public Sprite defaultSprite;
    [SerializeField] public Sprite selectedSprite;

    private void Awake()
    {
        InitializeComponents();
    }

    private void Start()
    {
        InitializeValues();
    }

    // Helper Functions --------------------------------------------------------

    private void InitializeComponents()
    {
        imageComponent = GetComponent<Image>();
    }

    private void InitializeValues()
    {
        defaultSprite = imageComponent.sprite;
    }

    public void IsSelected(bool value)
    {
        imageComponent.sprite = value ? selectedSprite : defaultSprite;
    }
}
