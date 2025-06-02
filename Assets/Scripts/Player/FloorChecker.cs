using UnityEngine;

public class FloorChecker : MonoBehaviour
{
    public bool onGround;
    private int groundContacts = 0;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Floor")
        {
            groundContacts++;
            onGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Floor")
        {
            groundContacts--;
            if (groundContacts <= 0)
            {
                groundContacts = 0;
                onGround = false;
            }
        }
    }
}
