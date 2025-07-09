using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Dialogue
{
	public Sprite character;
	public string name = "Default Name";
	public AudioClip soundToPlay;
	public TMP_FontAsset font;
	public float textSpeed = 0.024f;
	public float textPunctSpeed = 0.18f;
	public float lettersUntilSFX = 3;

	[TextArea(3, 10)]
	public string[] sentences;
	
}	