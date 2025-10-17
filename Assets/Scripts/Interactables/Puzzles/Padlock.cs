using UnityEngine;

public class Padlock : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private PadlockButton[] padlockButtons;
    [SerializeField] private string correctCombination;

    private void Start()
    {
        foreach (var button in padlockButtons) button.AssignPadlock(this);
    }

    public void CheckCombination()
    {
        string current = "";

        foreach (var button in padlockButtons) current += button.currentIndex.ToString();

        if (current == correctCombination) Debug.Log("PASS!");
        else Debug.Log("Current: " + current);
    }
}
