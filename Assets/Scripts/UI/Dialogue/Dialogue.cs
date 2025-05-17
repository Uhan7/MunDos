using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Dialogue
{
	public Sprite character;
	public string name;
	public AudioClip soundToPlay;
	public TMP_FontAsset font;
	public float textSpeed = 0.03f;
	public float textPunctSpeed = 0.1f;

	[TextArea(3, 10)]
	public string[] sentences;
	
}	