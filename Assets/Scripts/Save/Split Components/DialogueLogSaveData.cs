using System.Collections.Generic;

[System.Serializable]
public class DialogueLogSaveData : ComponentSaveData
{
    public List<string> storedDialogues = new();

    public DialogueLogSaveData()
    {
        type = "DialogueLog";
    }
}