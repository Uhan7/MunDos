using UnityEngine;
using UnityEngine.UI;

public class ZoomEnviManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject zoomEnviBackdrop;
    [SerializeField] private GameObject zoomEnviImage;

    public void ActivateZoomedEnvi(Sprite zoomSprite)
    {
        zoomEnviImage.GetComponent<Image>().sprite = zoomSprite;

        zoomEnviBackdrop.SetActive(true);
        zoomEnviImage.SetActive(true);
    }

    // I put this as a separate function since it might be executed differently soon
    public void DeactivateZoomedEnvi()
    {
        zoomEnviBackdrop.SetActive(false);
        zoomEnviImage.SetActive(false);
    }
}
