[System.Serializable]

public class DialogueManagerSaveData : ComponentSaveData
{
    public bool dialogueManagerWasClicked;
    public bool dialogueManagerOpen;
    public bool dialogueManagerSkip;
    public bool dialogueManagerCanNext;
    public bool dialogueManagerCanClick;
    public bool dialogueManagerMainCharacterIsSpeaking;

    public DialogueManagerSaveData()
    {
        type = "DialogueManager";
    }
}
