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
	[SerializeField] private Image character1Back;
	[SerializeField] private Image character2;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI dialogueText;
	[SerializeField] private TMP_FontAsset defaultFont;
	[SerializeField] private GameObject nextIndicator;
	[SerializeField] private Sprite emptySprite;
	[SerializeField] private LogScreen dialogueLog;

	[Header("Other Scripts")]
	[HideInInspector] private CameraEffectsManager cameraEffectsManager;

	[Header("Other Variables")]
    public bool playClosingAnimation = false;
    [HideInInspector] private float textSpeed;
	[HideInInspector] private float textPunctSpeed;
	[HideInInspector] private int lettersUntilSFX = 4;
	[HideInInspector] private AudioClip soundToPlay;

	[Header("Flags")]
	[HideInInspector] public bool open; // Used in Animator
	[HideInInspector] private bool skip;
	[HideInInspector] private bool canNext;
	[HideInInspector] public bool canClick; // Used in GameManager.cs
    [HideInInspector] public bool mainCharacterIsSpeaking; // Set in DialogueTrigger.cs

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
		dialogueLog.RecieveDialogue(dialogue);

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
		string cleanSentence = sentence
			.Replace("<shake>", "")
			.Replace("<flash>", "")
			.Replace("<dim>", "")
			.Replace("<startshake>", "")
			.Replace("<startflash>", "")
			.Replace("<startdim>", "")
			.Replace("<endshake>", "")
			.Replace("<endflash>", "")
			.Replace("<enddim>", "");

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

			// Manually putting the shi :( Will try to revise this soon real (tho not that big prio)

			if (originalCounter < sentence.Length && sentence.Substring(originalCounter).StartsWith("<shake>"))
			{
				cameraEffectsManager.ShakeScreen();
				originalCounter += 7;
				continue;
			}

			if (originalCounter < sentence.Length && sentence.Substring(originalCounter).StartsWith("<flash>"))
			{
				cameraEffectsManager.Flash();
				originalCounter += 7;
				continue;
			}

			if (originalCounter < sentence.Length && sentence.Substring(originalCounter).StartsWith("<dim>"))
			{
				cameraEffectsManager.Dim();
				originalCounter += 5;
				continue;
			}

			// CONTINUE THIS WITH THE LONG VERSIONS

			// End of manually putting shi

			if (counter % lettersUntilSFX == 0 && counter > 0)
			{
				aSource.pitch = Random.Range(0.9f, 1.1f);
				aSource.PlayOneShot(soundToPlay);
			}

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
		if (playClosingAnimation)
		{
			open = false;
			anim.SetBool("FullClose", true);
		} else
		{
            open = false;
            anim.SetBool("FullClose", false);
        }
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
        character1Back.sprite = dialogue.character;
		if (dialogue.character2 != null)
		{
			character2.sprite = dialogue.character2;
			character2.color = Color.gray;
		} else
		{
			character2.sprite = emptySprite;
		}
			soundToPlay = dialogue.soundToPlay;
        textSpeed = dialogue.textSpeed;
        textPunctSpeed = dialogue.textPunctSpeed;

		UpdateSpeaker();

        if (dialogue.font == null) dialogueText.font = defaultFont;
        else dialogueText.font = dialogue.font;

        nextIndicator.SetActive(false);
        gameObject.SetActive(true);
        open = true;

        dialogueText.text = " ";
    }

	public void UpdateSpeaker()
	{
		if (!mainCharacterIsSpeaking)
		{
			character.sprite = emptySprite;
			character2.color = Color.white;
		} else
		{
			character.sprite = character1Back.sprite;
            character2.color = Color.gray;
        }
	}
}