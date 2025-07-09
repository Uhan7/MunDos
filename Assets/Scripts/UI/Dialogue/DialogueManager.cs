using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	[Header("Components")]
	[HideInInspector] private Animator anim;
	[HideInInspector] private AudioSource aSource;
	[HideInInspector] private Queue<string> sentences;

	[Header("References")]
	[SerializeField] private Image character;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI dialogueText;
	[SerializeField] private TMP_FontAsset defaultFont;
	[SerializeField] private GameObject nextIndicator;

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
		dialogueText.text = "";

		foreach (char letter in sentence.ToCharArray())
		{
			if (!skip && !canNext)
			{
				dialogueText.text += letter;
				if (dialogueText.text.Length % lettersUntilSFX == 0) aSource.PlayOneShot(soundToPlay);

				float delay = (letter == '.' || letter == '?' || letter == '!' || letter == ',') ? textPunctSpeed : textSpeed;
				yield return new WaitForSeconds(delay);
			}

			if (skip)
			{
				dialogueText.text = sentence;
				break;
			}
		}

		if (dialogueText.text == sentence)
		{
			FinishSentence();
		}
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
		aSource = GetComponent<AudioSource>();
		sentences = new Queue<string>();
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