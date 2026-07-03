using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SettingsInfo
{
    public static bool debugMode = false;
    public static bool fromContinue = false;
    public static bool timePaused = false;

    public static float musicVol = 0.75f;
    public static float SFXVol = 0.75f;
    public static float dialogueVol = 1f;

    // Save slot information (bigger array is for when we have more than one)
    //public static int[,] saveSlots = new int[6, 2];
    public static int selectedSaveSlot = 0;
    public static int[,] saveSlots = new int[2, 2];
}
