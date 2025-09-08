using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	[Header("Components")]
	[HideInInspector] private Animator anim;
	[SerializeField] private AudioSource aSource;
	[HideInInspector] private Queue<string> sentences;

	[Header("References")]
	[SerializeField] private Image character;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI dialogueText;
	[SerializeField] private TMP_FontAsset defaultFont;
	[SerializeField] private GameObject nextIndicator;

	[Header("Other Scripts")]
	[HideInInspector] private CameraEffectsManager cameraEffectsManager;

	[Header("Other Variables")]
	[HideInInspector] private float textSpeed;
	[HideInInspector] private float textPunctSpeed;
	[HideInInspector] private int lettersUntilSFX = 3;
	[HideInInspector] private AudioClip soundToPlay;

	[Header("Flags")]
	[HideInInspector] public bool open; // Used in Animator
	[HideInInspector] private bool skip;
	[HideInInspector] private bool canNext;
	[HideInInspector] public bool canClick; // Used in GameManager.cs

	private void Awake()
    {
		InitializeComponents();
	}

    private void Start()
    {
		canClick = true;
    }

    private void Update()
    {
		anim.SetBool("Open", open);

		if (Input.GetMouseButtonUp(0) && canClick)
		{
			skip = true;
			if (canNext) DisplayNextSentence();
		}
	}

	// Coroutines --------------------------------------------------------------

    public IEnumerator StartDialogue(Dialogue dialogue)
	{
		InitializeDialogueValues(dialogue);

		yield return new WaitForSeconds(0.4f);
		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

        DisplayNextSentence();
    }

	IEnumerator TypeSentence(string sentence)
	{
		// Gonna have to manually add new shi here, Replaces the <thing> with empty
		string cleanSentence = sentence.Replace("<shake>", "").Replace("<flash>", "").Replace("<dim>", "");

		dialogueText.text = cleanSentence;
		dialogueText.maxVisibleCharacters = 0;

		yield return null;

		int totalVisibleCharacters = dialogueText.textInfo.characterCount;
		int counter = 0;
		int originalCounter = 0;

		while (counter <= totalVisibleCharacters)
		{
			if (skip)
			{
				dialogueText.maxVisibleCharacters = totalVisibleCharacters;
				break;
			}

			dialogueText.maxVisibleCharacters = counter;

			if (originalCounter < sentence.Length && sentence.Substring(originalCounter).StartsWith("<shake>"))
			{
				cameraEffectsManager.ShakeScreen();
				originalCounter += 7;
				continue;
			}

			if (counter % lettersUntilSFX == 0 && counter > 0) aSource.PlayOneShot(soundToPlay);

			if (counter > 0)
			{
				char c = dialogueText.text[dialogueText.textInfo.characterInfo[counter - 1].index];
				float delay = (c == '.' || c == '?' || c == '!' || c == ',') ? textPunctSpeed : textSpeed;
				yield return new WaitForSeconds(delay);
			}

			counter++;
			originalCounter++;
		}

		FinishSentence();
	}

	// Helper Functions --------------------------------------------------------

	public void DisplayNextSentence() // Used in DialogueTrigger.cs
	{
		canNext = false;
		skip = false;
		nextIndicator.SetActive(false);

		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	public void EndDialogue() // Used in DialogueTrigger.cs
	{
		skip = true;
		open = false;
		sentences.Clear();
		StopAllCoroutines();
	}

	void FinishSentence()
    {
		if (sentences.Count > 0) nextIndicator.SetActive(true);
		canNext = true;
	}

	void InitializeComponents()
    {
		anim = GetComponent<Animator>();
		sentences = new Queue<string>();
		cameraEffectsManager = GetComponent<CameraEffectsManager>();
	}

	void InitializeDialogueValues(Dialogue dialogue)
    {
        skip = false;
        canNext = false;

        nameText.text = dialogue.name;
        character.sprite = dialogue.character;
        soundToPlay = dialogue.soundToPlay;
        textSpeed = dialogue.textSpeed;
        textPunctSpeed = dialogue.textPunctSpeed;

        if (dialogue.font == null) dialogueText.font = defaultFont;
        else dialogueText.font = dialogue.font;

        nextIndicator.SetActive(false);
        gameObject.SetActive(true);
        open = true;

        dialogueText.text = " ";
    }

}