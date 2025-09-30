using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Dialogue
{
	public Sprite character;
	public Sprite character2;
	public string name = "Default Name";
	public AudioClip soundToPlay;
	public float audioPitchOffset = 0f;
	public TMP_FontAsset font;
	public float textSpeed = 0.024f;
	public float textPunctSpeed = 0.18f;
	public int lettersUntilSFX = 3;

	[TextArea(3, 10)]
	public string[] sentences;
	
}	