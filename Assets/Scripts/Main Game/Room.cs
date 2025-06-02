using UnityEngine;
using Unity.Cinemachine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject roomCamera;
    //[SerializeField] private float cameraSize;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player") return;

        roomCamera.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player") return;

        roomCamera.SetActive(false);
    }
}
