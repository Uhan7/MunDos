using UnityEngine;

public class sex : MonoBehaviour
{
    [SerializeField] private GameObject yuri;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip yuriSound;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            yuri.SetActive(false);
            yuri.SetActive(true);
            audioSource.PlayOneShot(yuriSound);
        }
    }
}
