using UnityEngine;
using Unity.Cinemachine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject roomCamera;
    //[SerializeField] private float cameraSize;

    [SerializeField] private GameObject exteriorBG;

    private Animator exteriorBGAnim;
    [SerializeField] private string roomFadeOutClip;
    [SerializeField] private string roomFadeInClip;

    private void Awake()
    {
        if (exteriorBG != null) exteriorBGAnim = exteriorBG.GetComponent<Animator>();
    }

    private void Start()
    {
        if (exteriorBG != null) exteriorBG.SetActive(true); // Might leave it inactive on build
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Protag") return;

        roomCamera.SetActive(true);
        if (exteriorBG != null) exteriorBGAnim.Play(roomFadeOutClip);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != "Protag") return;

        roomCamera.SetActive(false);
        if (exteriorBG != null) exteriorBGAnim.Play(roomFadeInClip);
    }
}
