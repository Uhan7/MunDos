using UnityEngine;

public class PadlockButton : MonoBehaviour
{

    [Header("References")]
    [HideInInspector] private Padlock padlock;

    [Header("Properties")]
    [SerializeField] private char[] possibleChars = { 'A', 'B', 'C' };
    [HideInInspector] private int currentIndex = 0;

    public void AssignPadlock(Padlock parent)
    {
        padlock = parent;
    }

    public void CycleButton()
    {
        currentIndex = (currentIndex + 1) % possibleChars.Length;
        Debug.Log($"Button {name} now shows {possibleChars[currentIndex]}");

        padlock.CheckCombination();
    }

    public char GetCurrentChar()
    {
        return possibleChars[currentIndex];
    }
}
