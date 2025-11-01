using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogScreen : MonoBehaviour
{
    [Header("Key Inputs")]
    [SerializeField] private KeyCode exitKey = KeyCode.Escape;
    [SerializeField] private KeyCode logKey;

    [SerializeField] private GameObject logScreen;
    [SerializeField] private TextMeshProUGUI fullLog;
    [SerializeField] private Scrollbar scrollbar;

    // Animation
    private Animator animator;
    private bool active = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(logKey) && !active)
        {
            toggleLogScreen();
            TogglePauseBackground();
        }
        if (Input.GetKeyDown(exitKey) && active)
        {
            toggleLogScreen();
            TogglePauseBackground();
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            scrollbar.value += 0.005f;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            scrollbar.value -= 0.005f;
        }
    }

    public void toggleLogScreen()
    {
        active = !active;
        logScreen.SetActive(!logScreen.activeInHierarchy);
    }

    public void RecieveDialogue(Dialogue dialogue)
    {
        string dialogueToAdd = "";

        foreach (string sentence in dialogue.sentences)
        {
            dialogueToAdd = "<b><u>" + dialogue.name.ToUpper() + "</b></u>\n";

            string cleanSentence = sentence.Replace("<shake>", "").Replace("<flash>", "").Replace("<dim>", "");

            dialogueToAdd = dialogueToAdd + cleanSentence;
            fullLog.text += dialogueToAdd + "\n\n";
        }
    }

    public void GoToMostRecentDialogue()
    {
        scrollbar.value = 0;
    }

    public void TogglePauseBackground()
    {
        EventBroadcaster.Instance.PostEvent(EventNames.TOGGLE_PAUSE_BG);
    }
}