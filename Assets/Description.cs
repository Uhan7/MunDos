using UnityEngine;

public class Description : MonoBehaviour
{
    [TextArea(6, 12)]
    [SerializeField] private string descriptionText = "Activated/Deactivated By:\n\nLocked/Unlocked By:\n\nConditional Check By:";
}
