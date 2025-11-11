using UnityEngine;
using NaughtyAttributes;

public class Oil : MonoBehaviour
{
    [Header("Oil Types")]
    [SerializeField] private bool isFromPast;

    [Header("Oil Values")]
    [HideIf("isFromPast")] [SerializeField] private float desiredPH;
    [HideIf("isFromPast")] [SerializeField] private float desiredSalinity;
    [HideIf("isFromPast")] [SerializeField] private float originalPH;
    [HideIf("isFromPast")] [SerializeField] private float originalSalinity;
    [ReadOnly] [HideIf("isFromPast")] [SerializeField] private float pH;
    [ReadOnly] [HideIf("isFromPast")] [SerializeField] private float salinity;
    [ShowIf("isFromPast")] [SerializeField] private Oil presentTimelineOil;

    [Header("Item Interact")]
    [HideIf("isFromPast")] [SerializeField] [TextArea(3, 10)] private string originalOrchidSapDZSentence = "Original Orchid Sap Sentence.";
    [HideIf("isFromPast")] [SerializeField] [TextArea(3, 10)] private string originalTubeDZSentence = "Original Oil Tube Sentence.";
    [ShowIf("isFromPast")] [SerializeField] [TextArea(3, 10)] private string originalHibiscusSapDZSentence = "Original Hibiscus Sap Sentence.";
    [ShowIf("isFromPast")] [SerializeField] [TextArea(3, 10)] private string originalHibiscusPetalDZSentence = "Original Hibiscus Petal Sentence.";
    [SerializeField] private GameObject DZ;
    [HideInInspector] private string DZSentence;

    [Header("References")]
    [HideIf("isFromPast")] [SerializeField] private BoxCollider2D[] otherOils;
    [HideIf("isFromPast")] [SerializeField] private GameObject correctDZ;

    [Header("Flags")]
    [HideInInspector] private bool finished = false;

    private void Start()
    {
        pH = originalPH;
        salinity = originalSalinity;
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

                DZSentence = originalOrchidSapDZSentence;
                break;

            case "Oil Tube":
                if (isFromPast)
                {
                    Debug.LogError("Error in Oil.cs: Oil Tube should NOT be isFromPast");
                    return;
                }

                DZSentence = originalTubeDZSentence.Replace("<oil_ph>", pH.ToString()).Replace("<oil_salinity>", salinity.ToString());

                if (pH == desiredPH && salinity == desiredSalinity) PuzzleFinish();
                break;

            case "Hibiscus Sap":
                if (!isFromPast)
                {
                    Debug.LogError("Error in Oil.cs: Hibiscus Sap should be isFromPast");
                    return;
                }
                presentTimelineOil.pH -= 0.1f;
                presentTimelineOil.salinity += 0.03f;

                DZSentence = originalHibiscusSapDZSentence;
                break;

            case "Hibiscus Petal":
                if (!isFromPast)
                {
                    Debug.LogError("Error in Oil.cs: Hibiscus Petal should be isFromPast");
                    return;
                }
                presentTimelineOil.pH = presentTimelineOil.originalPH;
                presentTimelineOil.salinity = presentTimelineOil.originalSalinity;

                DZSentence = originalHibiscusPetalDZSentence;
                break;

            default:
                break;
        }

        pH = Mathf.Round(pH * 100) / 100;
        salinity = Mathf.Round(salinity * 100) / 100;
        if (presentTimelineOil != null) presentTimelineOil.pH = Mathf.Round(presentTimelineOil.pH * 100) / 100;
        if (presentTimelineOil != null) presentTimelineOil.salinity = Mathf.Round(presentTimelineOil.salinity * 100) / 100;

        if (!finished)
        {
            DZ.GetComponent<DialogueTrigger>().dialogue.sentences[0] = DZSentence;
            DZ.SetActive(true);
        }
    }

    void PuzzleFinish()
    {
        foreach (BoxCollider2D col in otherOils) col.enabled = false;
        correctDZ.SetActive(true);

        finished = true;
    }
}
