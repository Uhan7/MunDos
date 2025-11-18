using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class type_out_text : MonoBehaviour
{
    private TextMeshProUGUI text;
    [SerializeField] private AudioSource aSource;

    [SerializeField] private bool perCharacter;
    [SerializeField] private bool perWord;

    [SerializeField] private AudioClip soundToPlay;
    [SerializeField] private int playPerLetter;

    [SerializeField] private float textSpeed;
    [SerializeField] private float textPunctSpeed;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (perCharacter) StartCoroutine(TypeSentence(text.text));
    }

    IEnumerator TypeSentence(string sentence)
    {
        text.text = "";
        foreach (char letter in sentence)
        {
            if (text.text.Length % playPerLetter == 0 && letter != ' ') aSource.PlayOneShot(soundToPlay);
            text.text += letter;
            if (letter == '.' || letter == '?' || letter == ',') yield return new WaitForSeconds(textPunctSpeed);
            else yield return new WaitForSeconds(textSpeed);
        }
    }
}
