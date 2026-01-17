using UnityEngine;

public class DialogueGuide : MonoBehaviour
{
    // Please attach this script to the actual parent of the dialogues, not the object. (Ex. [NPC] Dialogues, not the NPC itself)
    enum LevelArea
    {
        DG_Outpost,
        Forest,
        Town, 
        LiezelHouse,
        RommelHouse,
        Museum
    }

    enum DialogueType
    {
        Envi,
        Item,
        NPC,
        DZ
    }

    enum Timeline
    {
        Past,
        Present
    }

    // dialogueName : The name of what this dialogue is for, should match the LDD or the Unity object
    //                name that this script is attached to.
    [SerializeField] string dialogueName;

    // DIALOGUE INFORMATION
    [Header("General Information")]
        // levelFound : Just the level this dialogue is in. (1-3)
    [SerializeField] string levelFound;
        // timelineFound : If this instance of the dialogue is in the past or present.
    [SerializeField] Timeline timelineFound;
        // location : What part of the map this dialogue is in (choose closest answer).
    [SerializeField] LevelArea location;
        // typeOfDialogue : The kind of object this dialogue is attached to.
    [SerializeField] DialogueType typeOfDialogue;

    // STATE OF DIALOGUE
    [Header("State of Dialogue")]
        // isDone : Refers to if we consider the dialogue to be finished for the next submission, regardless of if it's
        //          planned for revision (since basically all dialogue is subject to revision).
    [SerializeField] public bool isDone;
        // beingRevised : If the dialogue is currently under the process of being revised, meaning if we expect it
        //                to change it soon. 
    [SerializeField] public bool beingRevised;

    // OTHER NOTES
    //[Header("Other")]
    [TextArea(1, 12)]
    [SerializeField] private string notes = "Add other notes here.";
}

