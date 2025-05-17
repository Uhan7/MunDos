using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	public Image chara;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI dialogueText;
	public TMP_FontAsset defaultFont;
	public float textSpeed;
	public float textPunctSpeed;
	public GameObject nextIndicator;

	private Queue<string> sentences;

	private Animator anim;
	public bool open;
	private bool skip;
	private bool canNext;

	private AudioSource aSource;
	private AudioClip soundToPlay;

	void Start()
	{
		aSource = GetComponent<AudioSource>();
		sentences = new Queue<string>();
		anim = GetComponent <Animator>();
	}

    private void Update()
    {
		anim.SetBool("Open", open);

		if (Input.GetMouseButtonDown(0))
		{
			skip = true;

			if (canNext)
			{
				canNext = false;
				skip = false;
				nextIndicator.SetActive(false);
				DisplayNextSentence();
			}
		}
	}

    public IEnumerator StartDialogue(Dialogue dialogue)
	{
		skip = false;
		canNext = false;

		nameText.text = dialogue.name;
		chara.sprite = dialogue.character;
		soundToPlay = dialogue.soundToPlay;
		textSpeed = dialogue.textSpeed;
		textPunctSpeed = dialogue.textPunctSpeed;
		if (dialogue.font == null) dialogueText.font = defaultFont;
		else dialogueText.font = dialogue.font;

		nextIndicator.SetActive(false);
		gameObject.SetActive(true);
		open = true;

		dialogueText.text = " ";

		yield return new WaitForSeconds(0.4f);
		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

        DisplayNextSentence();
    }

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence, textSpeed, textPunctSpeed));
	}

	IEnumerator TypeSentence(string sentence, float textSpeed, float textPunctSpeed)
	{
		dialogueText.text = "";
		yield return new WaitForSeconds(0.05f);
		foreach (char letter in sentence.ToCharArray())
		{
			if (!skip && !canNext)
			{
				if (dialogueText.text.Length % 3 == 0)
					aSource.PlayOneShot(soundToPlay);

				dialogueText.text += letter;

				float delay = (letter == '.' || letter == '?' || letter == '!' || letter == ',') ? textPunctSpeed : textSpeed;
				float timer = 0f;
				while (timer < delay && !skip)
				{
					timer += Time.deltaTime;
					yield return null;
				}
			}

			if (skip)
			{
				dialogueText.text = sentence;
				break;
			}
		}

		if (dialogueText.text == sentence)
		{
			if (sentences.Count > 0)
				nextIndicator.SetActive(true);
			canNext = true;
		}
	}

	public void EndDialogue()
	{
		skip = true;
		open = false;
		sentences.Clear();
		StopAllCoroutines();
	}

}