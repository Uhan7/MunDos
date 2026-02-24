using UnityEngine;
using NaughtyAttributes;

public class TextPrompt : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const string PROTAG_TAG = "Protag";

    [Header("Prompt Values")]
    [SerializeField] private GameObject promptBackground;
    [SerializeField] private GameObject textPrompt;
    [SerializeField] private bool deactivateAfter;
    [ShowIf("deactivateAfter")] [SerializeField] private TextPrompt[] otherHitboxesToSyncDeactivate;
    [SerializeField] private bool fullFadeIn;
    [SerializeField] private bool deactivateOnTimelineSwitch;

    private void OnDisable()
    {
        textPrompt.GetComponent<Animator>().Play("text_invisible", 0, 0f);
        promptBackground.GetComponent<Animator>().Play("image_invisible", 0, 0f);
    }

    private void OnEnable()
    {
        textPrompt.GetComponent<Animator>().Play("text_invisible", 0, 0f);
        promptBackground.GetComponent<Animator>().Play("image_invisible", 0, 0f);
    }

    private void Start()
    {
        promptBackground.GetComponent<Animator>().Play("image_invisible");
        textPrompt.GetComponent<Animator>().Play("text_invisible");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag(PROTAG_TAG)) return;

        if (fullFadeIn) promptBackground.GetComponent<Animator>().Play("image_fade_in_full");
        else promptBackground.GetComponent<Animator>().Play("image_fade_in_half");
        textPrompt.GetComponent<Animator>().Play("text_fade_in_full");
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag(PROTAG_TAG)) return;

        if (fullFadeIn) promptBackground.GetComponent<Animator>().Play("image_fade_out_full");
        else promptBackground.GetComponent<Animator>().Play("image_fade_out_half");
        textPrompt.GetComponent<Animator>().Play("text_fade_out_full");

        if (deactivateAfter && col.gameObject.activeInHierarchy && !deactivateOnTimelineSwitch)
        {
            DisableHitbox();
        }
        else if (deactivateAfter && deactivateOnTimelineSwitch)
        {
            DisableHitbox();
        }
    }

    public void DisableHitbox()
    {

        if (GetComponent<SpriteRenderer>() != null) GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        GetComponent<Collider2D>().enabled = false;

        if (otherHitboxesToSyncDeactivate != null) foreach (TextPrompt otherHitbox in otherHitboxesToSyncDeactivate) if (otherHitbox.gameObject.GetComponent<Collider2D>().enabled == true) otherHitbox.DisableHitbox();
    }
}
