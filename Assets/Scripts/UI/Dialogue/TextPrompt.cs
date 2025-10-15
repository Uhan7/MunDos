using UnityEngine;

public class TextPrompt : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const string PROTAG_TAG = "Protag";

    [Header("Prompt Values")]
    [SerializeField] private GameObject promptBackground;
    [SerializeField] private GameObject textPrompt;
    [SerializeField] private float timeUntilDeactivate;
    [SerializeField] private bool fullFadeIn;

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
    }
}
