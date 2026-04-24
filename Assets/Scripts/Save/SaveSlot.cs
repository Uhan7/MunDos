using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    // For the UI
    [SerializeField] public GameObject ribbon;
    [SerializeField] private Color defaultBackgroundColor;

    private Image screenshotSlot;
    private Image ribbonSprite;

    [SerializeField] private bool isSelected = false;
    [HideInInspector] public bool slotIsFull = false;
    [HideInInspector] public int timelineSaved = 0; // 0 past | 1 present

    void Awake()
    {
        screenshotSlot = GetComponent<Image>();
        ribbonSprite = ribbon.GetComponent<Image>();
    }

    public void SelectSlot()
    {
        isSelected = true;
    }

    public void UnselectSlot()
    {
        isSelected = false;
    }

    public void SetRibbon(Sprite newRibbon)
    {
        if (ribbon == null) Debug.Log("RIBBON IS NULL");
        if (newRibbon == null) Debug.Log("NEW RIBBON IS NULL");

        if (!ribbon.activeInHierarchy) ribbon.SetActive(true);
        if (ribbonSprite != null) ribbonSprite.sprite = newRibbon;
    }

    public void SetScreenshot(Sprite newBackground)
    {
        screenshotSlot.color = Color.white;
        screenshotSlot.sprite = newBackground;
    }

    public void ResetScreenshot()
    {
        screenshotSlot.color = defaultBackgroundColor;
    }
}
