using UnityEngine;
using UnityEngine.UI;

public class PadlockButton : MonoBehaviour
{

    [Header("References")]
    [HideInInspector] private Image imageComponent;
    [HideInInspector] private Padlock padlock;
    [SerializeField] private ClickZoomable clickZoomable;

    [Header("Properties")]
    [SerializeField] private Sprite[] buttonSprites;
    [HideInInspector] public int currentIndex = 0; // Used in Padlock.cs



    private void Awake()
    {
        imageComponent = GetComponent<Image>();
    }

    private void Start()
    {
        imageComponent.sprite = buttonSprites[currentIndex];
    }

    public void AssignPadlock(Padlock parent)
    {
        padlock = parent;
    }

    public void CycleButton()
    {
        currentIndex = (currentIndex + 1) % buttonSprites.Length;
        imageComponent.sprite = buttonSprites[currentIndex];

        if (padlock != null)
        {
            padlock.CheckCombination();
        }
    }

    public void PressButton()
    {
        UnityEngine.UI.Button button = this.gameObject.GetComponent<UnityEngine.UI.Button>();
        button.interactable = false;
        if (clickZoomable != null)
        {
            clickZoomable.CheckCombination(this.gameObject);
        }
        else
        {
            Debug.LogError($"clickzoom of {this.gameObject.name} is null");
        }
    }
}
