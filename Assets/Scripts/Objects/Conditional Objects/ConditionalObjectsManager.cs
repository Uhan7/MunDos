using UnityEngine;

public class ConditionalObjectsManager : MonoBehaviour
{
    public void AddCheck(GameObject conditionalObject) // Used in ConiditionalObjectsManager.cs
    {
        ConditionalObject conditionalObjectScript = conditionalObject.GetComponent<ConditionalObject>();

        conditionalObjectScript.currentChecks++;
        if (conditionalObjectScript.currentChecks >= conditionalObjectScript.requiredChecks) conditionalObject.SetActive(true);
    }
}
