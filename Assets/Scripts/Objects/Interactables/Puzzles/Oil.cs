using UnityEngine;
using NaughtyAttributes;

public class Oil : MonoBehaviour
{
    [Header("Oil Values")]
    [SerializeField] private float originalPH;
    [SerializeField] private float originalSalinity;
    [SerializeField] private float pH;
    [SerializeField] private float salinity;
    [SerializeField] private bool isFromPast;
    [HideIf("isFromPast")] [SerializeField] private GameObject pastTimelineOil;
    [ShowIf("isFromPast")] [SerializeField] private GameObject presentTimelineOil;

    [Header("Normal Interact")]
    [SerializeField] private GameObject DZ;
    [SerializeField] [TextArea(3, 10)] private string originalDZSentence = "A pH of <oil_ph> and Salinity of <oil_salinity>.";
    [HideInInspector] private string DZSentence;

    [Header("Item Interact")]
    [SerializeField] private GameObject itemDZ;
    [SerializeField] [TextArea(3, 10)] private string originalItemDZSentence = "It changed to a pH of <oil_ph> and Salinity of <oil_salinity>.";
    [HideInInspector] private string itemDZSentence;

    public void Interact()
    {
        DZSentence = originalDZSentence.Replace("<oil_ph>", pH.ToString()).Replace("<oil_salinity>", salinity.ToString());
        DZ.GetComponent<DialogueTrigger>().dialogue.sentences[0] = DZSentence;
        DZ.SetActive(true);
    }

    public void ItemInteract(string itemName)
    {
        switch (itemName)
        {
            case "Orchid Sap":
                pH += 0.3f;
                salinity -= 0.01f;
                break;

            case "Hibiscus Sap":
                pH -= 0.1f;
                salinity += 0.03f;
                break;

            case "Hibiscus Petal":
                pH = originalPH;
                salinity = originalSalinity;
                break;

            default: break;
        }

        itemDZSentence = originalItemDZSentence.Replace("<oil_ph>", pH.ToString()).Replace("<oil_salinity>", salinity.ToString());
        itemDZ.GetComponent<DialogueTrigger>().dialogue.sentences[0] = itemDZSentence;
        itemDZ.SetActive(true);
    }
}
