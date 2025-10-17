using UnityEngine;
using UnityEngine.UI;

public class PadlockButton : MonoBehaviour
{

    [Header("References")]
    [HideInInspector] private Image imageComponent;
    [HideInInspector] private Padlock padlock;

    [Header("Properties")]
    [SerializeField] private Sprite[] buttonSprites;
    [HideInInspector] public int currentIndex = 0; // Used in Padlock.cs

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
    }

    public void AssignPadlock(Padlock parent)
    {
        padlock = parent;
    }

    public void CycleButton()
    {
        currentIndex = (currentIndex + 1) % buttonSprites.Length;
        imageComponent.sprite = buttonSprites[currentIndex];

        padlock.CheckCombination();
    }
}
