using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Zoom Components")]
    [HideInInspector] float SCREEN_WIDTH;
    [HideInInspector] float SCREEN_HEIGHT;
    [HideInInspector] float REFERENCE_WIDTH = 1080;
    [HideInInspector] float REFERENCE_HEIGHT = 1920;
    [HideInInspector] float REFERENCE_RATIO;
    [HideInInspector] float ASPECT_RATIO;
    [HideInInspector] float ZOOM_PERCENTAGE = 0.70f;


    [Header("Components")]
    [HideInInspector] private Image imageComponent;
    [SerializeField] public GameObject zoomObject;

    [Header("ItemData Variables")]
    [HideInInspector] public ItemData data; // Used by InventoryManager.cs

    [Header("Additional Slot Data")]
    [SerializeField] public Sprite defaultSprite;
    [SerializeField] public Sprite selectedSprite;

    [Header("Interactions")]
    [SerializeField] private GameObject[] toActivateOnZoom;
    [SerializeField] private GameObject[] toDeactivateOnZoom;

    bool test = false;
    private void Awake()
    {
        if (gameObject.activeInHierarchy) InitializeComponents();
    }

    private void OnEnable()
    {
        if (imageComponent == null) InitializeComponents();
    }

    private void Update()
    {
        if (Screen.width != SCREEN_WIDTH) 
        {
            SCREEN_WIDTH = Screen.width;
            test = true;
        }
        if (Screen.height != SCREEN_HEIGHT) 
        {
            SCREEN_HEIGHT = Screen.height;
            test = true;
        }
        if (test) 
        { 
            Debug.Log($"screen width {SCREEN_WIDTH} height {SCREEN_HEIGHT}, % width {SCREEN_WIDTH * ZOOM_PERCENTAGE}, height {SCREEN_HEIGHT * ZOOM_PERCENTAGE}"); 
            test = false; 
            ASPECT_RATIO = REFERENCE_WIDTH / REFERENCE_HEIGHT;
        }
    }
    private void Start()
    {
        REFERENCE_RATIO = REFERENCE_WIDTH / REFERENCE_HEIGHT;
        SCREEN_WIDTH = Screen.width;
        SCREEN_HEIGHT = Screen.height;
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
        zoomObject.GetComponent<UnityEngine.UI.Image>().sprite = this.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite;
    }
    public void SetZoomState(bool value)
    {
        Focus(value);
        zoomObject.SetActive(value);
        Sprite sprite = zoomObject.GetComponent<UnityEngine.UI.Image>().sprite;
        //RectTransform rectTransform = sprite.GetComponent<RectTransform>();
        ////image.preserveAspect = true;
        //Vector2 imageSize = rectTransform.sizeDelta;
        //float factor;
        //if (imageSize.y >= imageSize.x)
        //{
        //    factor = (SCREEN_WIDTH * ZOOM_PERCENTAGE) / imageSize.y;
        //}
        //else
        //{
        //    factor = (SCREEN_HEIGHT * ZOOM_PERCENTAGE) / imageSize.x;
        //}

        //imageSize.x = imageSize.x * factor;
        //imageSize.y = imageSize.y * factor;
        //rectTransform.sizeDelta = new Vector2(imageSize.x, imageSize.y);
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
