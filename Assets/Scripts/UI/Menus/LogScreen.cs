using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogScreen : MonoBehaviour
{
    [SerializeField] private GameObject logScreen;
    [SerializeField] private TextMeshProUGUI fullLog;
    [SerializeField] private Scrollbar scrollbar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            scrollbar.value += 0.0001f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            scrollbar.value -= 0.0001f;
        }
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