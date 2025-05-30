using UnityEngine;

public class FloorChecker : MonoBehaviour
{
    public bool onGround;

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Floor":
            case "Somethin else idk":
                onGround = true; break;

            default:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Floor":
            case "Somethin else idk":
                onGround = false; break;

            default:
                break;
        }
    }
}
