using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class DialogueManager : MonoBehaviour
{
	[Header("Components")]
	[HideInInspector] private Animator anim;
	[SerializeField] private AudioSource aSource;
	[SerializeField] private AudioSource dSource;
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
    [HideInInspector] public bool playClosingAnimation = false;
    [HideInInspector] private float textSpeed;
	[HideInInspector] private float textPunctSpeed;
	[HideInInspector] private int lettersUntilSFX = 4;
	[HideInInspector] private AudioClip soundToPlay;
	[SerializeField] private AudioClip continueDialogueSFX;
	[SerializeField] private float minPitch = 0.9f;
	[SerializeField] private float maxPitch = 1.1f;

	[Header("Effects Variables")]
	[HideInInspector] private int totalVisibleCharacters;
	[HideInInspector] private int counter;
	[HideInInspector] private int originalCounter;

	[Header("Flags")]
	[HideInInspector] private bool wasClicked = false;
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

		if ((Input.GetKey(KeyCode.BackQuote) || wasClicked) && canClick && SettingsInfo.debugMode)
		{
			wasClicked = false;
			skip = true;
			if (canNext) DisplayNextSentence();
		}

		if ((Input.GetKeyDown(KeyCode.Space) || wasClicked) && canClick)
		{
			wasClicked = false;
			skip = true;
			if (canNext)
			{
				if (continueDialogueSFX != null) aSource.PlayOneShot(continueDialogueSFX);
				DisplayNextSentence();
			}
		}
	}

	// Coroutines --------------------------------------------------------------

    public IEnumerator StartDialogue(Dialogue dialogue)
	{
		InitializeDialogueValues(dialogue);
		dialogueLog.RecieveName(dialogue);

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
			.Replace("<start_shake>", "")
			.Replace("<end_shake>", "")
			.Replace("<flash>", "")
			.Replace("<start_flash>", "")
			.Replace("<end_flash>", "")
			.Replace("<dim>", "")
			.Replace("<start_dim>", "")
			.Replace("<end_dim>", "")
			.Replace("<end_all>", "")
			.Replace("<cut>", "");

		cleanSentence = Regex.Replace(sentence, @"<SFX_[^ >]+>", "");

		dialogueText.text = cleanSentence;
		dialogueText.maxVisibleCharacters = 0;

		// Gives the sentence to the log screen
        dialogueLog.RecieveDialogue(cleanSentence);

		yield return null;

		totalVisibleCharacters = dialogueText.textInfo.characterCount;
		counter = 0;
		originalCounter = 0;

		while (counter <= totalVisibleCharacters)
		{
			if (skip)
			{
				dialogueText.maxVisibleCharacters = totalVisibleCharacters;
				break;
			}

			dialogueText.maxVisibleCharacters = counter;

			// Depending on effect, we will play it (the conditional is inside)
			DialogueCameraEffect(sentence, "<shake>", "ShakeScreen");
			DialogueCameraEffect(sentence, "<start_shake>", "StartShakeScreen");
			DialogueCameraEffect(sentence, "<end_shake>", "EndShakeScreen");
			DialogueCameraEffect(sentence, "<flash>", "Flash");
			DialogueCameraEffect(sentence, "<start_flash>", "StartFlash");
			DialogueCameraEffect(sentence, "<end_flash>", "EndFlash");
			DialogueCameraEffect(sentence, "<dim>", "Dim");
			DialogueCameraEffect(sentence, "<start_dim>", "StartDim");
			DialogueCameraEffect(sentence, "<end_dim>", "EndDim");
			DialogueCameraEffect(sentence, "<end_all>", "EndAll");
			CutOffDialogue(sentence);

			// Character voices

			// Santi
			{
				DialogueAudioEffect(sentence, "<SFX_santi_concerned>");
				DialogueAudioEffect(sentence, "<SFX_santi_frustrated>");
				DialogueAudioEffect(sentence, "<SFX_santi_happy>");
				DialogueAudioEffect(sentence, "<SFX_santi_oh>");
				DialogueAudioEffect(sentence, "<SFX_santi_questioning>");
				DialogueAudioEffect(sentence, "<SFX_santi_relieved>");
				DialogueAudioEffect(sentence, "<SFX_santi_satisfied>");
				DialogueAudioEffect(sentence, "<SFX_santi_shock_gasp>");
				DialogueAudioEffect(sentence, "<SFX_santi_sigh>");
				DialogueAudioEffect(sentence, "<SFX_santi_thinking>");
				DialogueAudioEffect(sentence, "<SFX_santi_wondering>");
			}

            // Liezel
            {
				DialogueAudioEffect(sentence, "<SFX_liezel_concerned>");
				DialogueAudioEffect(sentence, "<SFX_liezel_discovery>");
				DialogueAudioEffect(sentence, "<SFX_liezel_frustrated>");
				DialogueAudioEffect(sentence, "<SFX_liezel_happy>");
				DialogueAudioEffect(sentence, "<SFX_liezel_oh>");
				DialogueAudioEffect(sentence, "<SFX_liezel_questioning>");
				DialogueAudioEffect(sentence, "<SFX_liezel_relieved>");
				DialogueAudioEffect(sentence, "<SFX_liezel_satisfied_(aha!)>");
				DialogueAudioEffect(sentence, "<SFX_liezel_satisfied_(ha!)>");
				DialogueAudioEffect(sentence, "<SFX_liezel_shock_gasp>");
				DialogueAudioEffect(sentence, "<SFX_liezel_thinking>");
				DialogueAudioEffect(sentence, "<SFX_liezel_wondering>");
			}

			// Antiquarian
			{
				DialogueAudioEffect(sentence, "<SFX_antiquarian_frustrated_grunt>");
				DialogueAudioEffect(sentence, "<SFX_antiquarian_normal_suspicious>");
				DialogueAudioEffect(sentence, "<SFX_antiquarian_ominous_chuckle_1>");
				DialogueAudioEffect(sentence, "<SFX_antiquarian_ominous_chuckle_2>");
				DialogueAudioEffect(sentence, "<SFX_antiquarian_proud>");
				DialogueAudioEffect(sentence, "<SFX_antiquarian_satisfied>");
				DialogueAudioEffect(sentence, "<SFX_antiquarian_snicker>");
				DialogueAudioEffect(sentence, "<SFX_antiquarian_surprised_gasp>");
				DialogueAudioEffect(sentence, "<SFX_antiquarian_thinking>");
			}

			// Ate Guard
			{
				DialogueAudioEffect(sentence, "<SFX_antiquarian_frustrated_grunt>");
				DialogueAudioEffect(sentence, "<SFX_antiquarian_normal_suspicious>");
				DialogueAudioEffect(sentence, "<SFX_antiquarian_ominous_chuckle_1>");
				DialogueAudioEffect(sentence, "<SFX_antiquarian_ominous_chuckle_2>");
				DialogueAudioEffect(sentence, "<SFX_antiquarian_proud>");
				DialogueAudioEffect(sentence, "<SFX_antiquarian_satisfied>");
			}

			// Distressed Woman
			{
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_annoyed>");
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_crying_1>");
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_crying_2>");
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_disgust>");
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_frustrated>");
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_normal>");
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_sigh>");
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_surprised_gasp>");
			}

			// Distressed Woman
			{
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_annoyed>");
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_crying_1>");
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_crying_2>");
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_disgust>");
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_frustrated>");
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_normal>");
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_sigh>");
				DialogueAudioEffect(sentence, "<SFX_distressed_woman_surprised_gasp>");
			}

            // Manong Guard
            {
				DialogueAudioEffect(sentence, "<SFX_manong_guard_frustrated>");
				DialogueAudioEffect(sentence, "<SFX_manong_guard_normal>");
				DialogueAudioEffect(sentence, "<SFX_manong_guard_proud>");
				DialogueAudioEffect(sentence, "<SFX_manong_guard_sigh>");
				DialogueAudioEffect(sentence, "<SFX_manong_guard_thinking>");
			}

			if (counter % lettersUntilSFX == 0 && counter > 0)
			{
				dSource.pitch = Random.Range(minPitch, maxPitch);
				dSource.PlayOneShot(soundToPlay);
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
		nextIndicator.SetActive(true);
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

		minPitch = 0.9f;
		maxPitch = 1.1f;
		minPitch += dialogue.audioPitchOffset;
		maxPitch += dialogue.audioPitchOffset;

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

	void DialogueCameraEffect(string sentence, string substring, string functionName)
    {
		if (originalCounter < sentence.Length && sentence.Substring(originalCounter).StartsWith(substring))
		{
			cameraEffectsManager.Invoke(functionName, 0f);
			originalCounter += substring.Length;
		}
	}

	void DialogueAudioEffect(string sentence, string substring)
	{
		if (originalCounter < sentence.Length && sentence.Substring(originalCounter).StartsWith(substring))
		{
			cameraEffectsManager.PlaySFX(substring);
			originalCounter += substring.Length;
		}
	}

	void CutOffDialogue(string sentence)
    {
		if (counter >= totalVisibleCharacters && sentence.Contains("<cut>"))
		{
			EndDialogue();
		}
	}

	// Stupid Functions --------------------------------------------------------

	public void RaycastTester()
    {
		print("Hover");
    }

	public void SetWasClicked(bool val)
    {
		wasClicked = val;
    }

}