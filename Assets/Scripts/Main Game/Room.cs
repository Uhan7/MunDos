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
        exteriorBGAnim = exteriorBG.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Protag") return;

        roomCamera.SetActive(true);
        exteriorBGAnim.Play(roomFadeOutClip);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != "Protag") return;

        roomCamera.SetActive(false);
        exteriorBGAnim.Play(roomFadeInClip);
    }
}
