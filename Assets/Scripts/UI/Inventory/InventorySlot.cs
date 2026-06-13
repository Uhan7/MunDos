using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector] private Image imageComponent;
    [SerializeField] public GameObject zoomObject;
    [SerializeField] private GameObject backDrop;

    [Header("ItemData Variables")]
    [HideInInspector] public ItemData data; // Used by InventoryManager.cs

    [Header("Additional Slot Data")]
    [SerializeField] public Sprite defaultSprite;
    [SerializeField] public Sprite selectedSprite;

    [Header("Interactions")]
    [SerializeField] private GameObject[] toActivateOnZoom;
    [SerializeField] private GameObject[] toDeactivateOnZoom;

    private void Awake()
    {
        InitializeComponents();
    }

    private void OnEnable()
    {
        if (imageComponent == null) InitializeComponents();
    }

    private void Start()
    {
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
    
    public void SetZoomObject()
    {
        zoomObject.GetComponent<Image>().sprite = this.transform.GetChild(0).GetComponent<Image>().sprite;
    }
    public void SetZoomState(bool value)
    {
        Focus(value);
        zoomObject.SetActive(value);
        backDrop.SetActive(value);
    }
    public void OnZoom()
    {
        SetAll(toActivateOnZoom, true);
        SetAll(toDeactivateOnZoom, false);
    }
    void Focus(bool value)
    {
        Parameters param = new Parameters();
        param.PutExtra(ParamNames.IS_FOCUSING_DIALOGUE, value);

        EventBroadcaster.Instance.PostEvent(EventNames.FOCUS_DIALOGUE, param);
    }
    void SetAll(GameObject[] objects, bool value)
    {
        if (objects == null) return;
        foreach (GameObject obj in objects)
        {
            if (obj == null) continue;
            obj.SetActive(value);
        }
    }
}
