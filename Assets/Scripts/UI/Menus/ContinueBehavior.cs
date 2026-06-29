using UnityEngine;

public class ContinueBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject loadScreen;
    [SerializeField] GameObject noSaveWarning;
    [SerializeField] SaveManager saveManager;

    public void ActivateLoadScreen()
    {
        if (saveManager.CheckSaveExists())
        {
            loadScreen.SetActive(true);
        } else
        {
            noSaveWarning.GetComponent<Animator>().ResetTrigger("Appear");
            noSaveWarning.GetComponent<Animator>().SetTrigger("Appear");
        }
    }
}
