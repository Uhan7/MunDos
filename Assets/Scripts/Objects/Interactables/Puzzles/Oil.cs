using UnityEngine;
using NaughtyAttributes;

public class Oil : MonoBehaviour
{
    [Header("Oil Values")]
    [SerializeField] private float pH;
    [SerializeField] private float salinity;
    [SerializeField] private bool isFromPast;
    [HideIf("isFromPast")] [SerializeField] private GameObject pastTimelineOil;
    [HideIf("isFromPast")] [SerializeField] private GameObject DZ;
    [SerializeField] [TextArea(3, 10)] private string originalDZSentence = "A pH of <pH> and Salinity of <Salinity>.";
    [HideInInspector] private string DZSentence;

    public void Interact()
    {
        DZSentence = originalDZSentence.Replace("<pH>", "<b>"+pH.ToString()+"</b>").Replace("<Salinity>", "<b>"+salinity.ToString()+"</b>");
        DZ.GetComponent<DialogueTrigger>().dialogue.sentences[0] = DZSentence;
        DZ.SetActive(true);
    }
}
