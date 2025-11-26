using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Key Inputs")]
    [SerializeField] private KeyCode exitKey = KeyCode.Escape;
    [SerializeField] private KeyCode logKey;

    [Header("References")]
    [SerializeField] private GameManager gameManagerReference;
    [SerializeField] private GameObject logScreen;

    [Header("Parts")]
    [SerializeField] private TextMeshProUGUI fullLog;
    [SerializeField] private Scrollbar scrollbar;

    [Header("Audio")]
    [SerializeField] private AudioClip logOpenSFX;
    [SerializeField] private AudioClip logCloseSFX;

    [Header("Limits")]
    [SerializeField] private List<string> storedDialogues = new();
    [SerializeField] private int maxDialoguesToStore = 100;

    // Animation
    private bool active = false;

    private string speakerName = "";

    private void Update()
    {
        if (gameManagerReference.isHidingUI) return;
        if (Input.GetKey(KeyCode.Tab)) return;

        if (Input.GetKeyDown(logKey) && gameManagerReference.canPause)
        {
            LogScreenOn();
        }
        else if (Input.GetKeyDown(logKey) && !gameManagerReference.canPause)
        {
            LogScreenOff();
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
        if (active)
        {
            StartCoroutine(UpdateMostRecent());
        }

        if (sfxSource != null) sfxSource.PlayOneShot(active ? logOpenSFX : logCloseSFX);
    }

    public void LogScreenOn()
    {
        if (gameManagerReference.canPause && !active)
        {
            gameManagerReference.canPause = false;
            toggleLogScreen();
            TogglePauseBackground();
        }
    }

    public void LogScreenOff()
    {
        if (active)
        {
            gameManagerReference.canPause = true;
            toggleLogScreen();
            TogglePauseBackground();
        }
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
        storedDialogues.Add(dialogueToAdd);
        if (storedDialogues.Count > 100) storedDialogues.RemoveAt(0);

        fullLog.text = "";
        foreach (string dialogueLine in storedDialogues)
        {
            fullLog.text += dialogueLine + "<size=\"26\">\n\n";
        }
    }

    public void GoToMostRecentDialogue()
    {
        scrollbar.value = 0;
    }

    private IEnumerator UpdateMostRecent()
    {
        yield return new WaitForEndOfFrame();
        GoToMostRecentDialogue();
    }

    public void TogglePauseBackground()
    {
        if (gameManagerReference.isHidingUI) return;

        EventBroadcaster.Instance.PostEvent(EventNames.TOGGLE_PAUSE_BG);
    }
}