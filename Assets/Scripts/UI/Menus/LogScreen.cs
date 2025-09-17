using TMPro;
using UnityEngine;

public class LogScreen : MonoBehaviour
{
    [SerializeField] private GameObject logScreen;
    [SerializeField] private TextMeshProUGUI fullLog;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void toggleLogScreen()
    {
        logScreen.SetActive(!logScreen.activeInHierarchy);
    }

    public void RecieveDialogue(Dialogue dialogue)
    {
        string dialogueToAdd = "";

        foreach (string sentence in dialogue.sentences)
        {
            dialogueToAdd = "<b><u>" + dialogue.name + "</b></u>\n";
            dialogueToAdd = dialogueToAdd + sentence;
            fullLog.text += dialogueToAdd + "\n\n";
        }
    }
}