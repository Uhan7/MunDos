using UnityEngine;
using NaughtyAttributes;

public class Oil : MonoBehaviour
{
    [Header("Oil Values")]
    [SerializeField] private bool isFromPast;
    [HideIf("isFromPast")] [SerializeField] private float originalPH;
    [HideIf("isFromPast")] [SerializeField] private float originalSalinity;
    [HideIf("isFromPast")] [SerializeField] private float pH;
    [HideIf("isFromPast")] [SerializeField] private float salinity;
    [HideIf("isFromPast")] [SerializeField] private Oil pastTimelineOil;
    [ShowIf("isFromPast")] [SerializeField] private Oil presentTimelineOil;

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
                if (isFromPast)
                {
                    Debug.LogError("Error in Oil.cs: Orchid Sap should NOT be isFromPast");
                    return;
                }
                pH += 0.3f;
                salinity -= 0.01f;
                break;

            case "Hibiscus Sap":
                if (!isFromPast)
                {
                    Debug.LogError("Error in Oil.cs: Hibiscus Sap should be isFromPast");
                    return;
                }
                presentTimelineOil.pH -= 0.1f;
                presentTimelineOil.salinity += 0.03f;
                break;

            case "Hibiscus Petal":
                if (!isFromPast)
                {
                    Debug.LogError("Error in Oil.cs: Hibiscus Petal should be isFromPast");
                    return;
                }
                presentTimelineOil.pH = originalPH;
                presentTimelineOil.salinity = originalSalinity;
                break;

            default: break;
        }

        pH = Mathf.Round(pH * 100) / 100;
        salinity = Mathf.Round(salinity * 100) / 100;
        presentTimelineOil.pH = Mathf.Round(presentTimelineOil.pH * 100) / 100;
        presentTimelineOil.salinity = Mathf.Round(presentTimelineOil.salinity * 100) / 100;

        itemDZSentence = originalItemDZSentence.Replace("<oil_ph>", pH.ToString()).Replace("<oil_salinity>", salinity.ToString());
        itemDZ.GetComponent<DialogueTrigger>().dialogue.sentences[0] = itemDZSentence;
        itemDZ.SetActive(true);
    }
}
