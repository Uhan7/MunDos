using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogScreen : MonoBehaviour
{
    [Header("Key Inputs")]
    [SerializeField] private KeyCode exitKey = KeyCode.Escape;
    [SerializeField] private KeyCode logKey;

    [SerializeField] private GameManager gameManagerReference;
    [SerializeField] private GameObject logScreen;
    [SerializeField] private TextMeshProUGUI fullLog;
    [SerializeField] private Scrollbar scrollbar;

    // Animation
    private bool active = false;

    private string speakerName = "";

    private void Update()
    {
        if (Input.GetKeyDown(logKey) && !active)
        {
            toggleLogScreen();
            TogglePauseBackground();
        }
        if (Input.GetKeyDown(logKey) && active)
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
        if (gameManagerReference.isHidingUI) return;

        active = !active;
        logScreen.SetActive(!logScreen.activeInHierarchy);
    }

    public void RecieveName(Dialogue dialogue)
    {
        speakerName = dialogue.name.ToUpper();
    }

    public void RecieveDialogue(string sentence)
    {
        string dialogueToAdd = "";

        dialogueToAdd = "<b><u>" + speakerName + "</b></u>\n";

        dialogueToAdd = dialogueToAdd + sentence;
        fullLog.text += dialogueToAdd + "<size=\"26\">\n\n";
    }

    public void GoToMostRecentDialogue()
    {
        scrollbar.value = 0;
    }

    public void TogglePauseBackground()
    {
        if (gameManagerReference.isHidingUI) return;

        EventBroadcaster.Instance.PostEvent(EventNames.TOGGLE_PAUSE_BG);
    }
}