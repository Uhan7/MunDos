[System.Serializable]

public class DialogueTriggerSaveData : ComponentSaveData
{
    public bool dialogueIsTriggered;
    public bool dialogueAlreadyActivatedObjects;
    public bool dialogueAlreadyDeactivatedObjects;
    public bool dialogueAlreadyCheckedConditional;
    public bool dialogueAlreadyRemovedPlayeritems;
    public bool dialogueAlreadyGavePlayeritems;

    public DialogueTriggerSaveData()
    {
        type = "DialogueTrigger";
    }
}
