using UnityEngine;

public class Padlock : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private PadlockButton[] padlockButtons;
    [SerializeField] private string correctCombination;

    [Header("Components")]
    [SerializeField] private bool deactivateAfter;
    [SerializeField] private GameObject[] toActivate;
    [SerializeField] private GameObject[] toDeactivate;

    private void Start()
    {
        foreach (var button in padlockButtons) button.AssignPadlock(this);
    }

    public void CheckCombination()
    {
        string current = "";

        foreach (var button in padlockButtons) current += button.currentIndex.ToString();

        if (current == correctCombination) 
        {
            Debug.Log("PASS!");
            SetAll(toActivate, true);
            SetAll(toDeactivate, false);
        } 
        else Debug.Log("Current: " + current);
    }

    void SetAll(GameObject[] objects, bool value)
    {
        if (objects == null || objects.Length == 0) return;
        foreach (GameObject obj in objects) obj.SetActive(value);
    }
}
