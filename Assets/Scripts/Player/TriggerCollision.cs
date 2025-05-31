using UnityEngine;

public class TriggerCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Enter");
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exit");
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Trigger Stay");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Enter");
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Collision Exit");
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("Collision Stay");
    }
}
