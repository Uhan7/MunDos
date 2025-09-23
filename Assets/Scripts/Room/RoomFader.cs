using UnityEngine;

public class RoomFader : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.Play("room_fade_in");
    }

    private void OnDisable()
    {
        anim.Play("room_fade_out");
    }
}
