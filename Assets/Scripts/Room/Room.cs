using UnityEngine;
using Unity.Cinemachine;
using NaughtyAttributes;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject roomCamera;
    [SerializeField] private bool isEnclosed;
    [ShowIf("isEnclosed")] [SerializeField] private GameObject exteriorWalls;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Protag") return;

        roomCamera.GetComponent<CinemachineCamera>().Target.TrackingTarget = col.gameObject.transform;
        roomCamera.SetActive(true);
        if (exteriorWalls != null) exteriorWalls.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != "Protag") return;

        roomCamera.SetActive(false);
        if (exteriorWalls != null) exteriorWalls.SetActive(true);
    }
}
