using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogScreen : MonoBehaviour
{
    [Header("Key Inputs")]
    [SerializeField] private KeyCode exitKey = KeyCode.Escape;

    [SerializeField] private GameObject logScreen;
    [SerializeField] private TextMeshProUGUI fullLog;
    [SerializeField] private Scrollbar scrollbar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(exitKey)) toggleLogScreen();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            scrollbar.value += 0.005f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            scrollbar.value -= 0.005f;
        }
    }
    public void toggleLogScreen()
    {
        logScreen.SetActive(!logScreen.activeInHierarchy);
        GoToMostRecentDialogue();
    }

    public void RecieveDialogue(Dialogue dialogue)
    {
        string dialogueToAdd = "";

        foreach (string sentence in dialogue.sentences)
        {
            dialogueToAdd = "<b><u>" + dialogue.name + "</b></u>\n";

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