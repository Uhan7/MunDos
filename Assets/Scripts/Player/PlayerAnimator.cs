using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private void Awake()
    {
        spriteRenderer= GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAnimationParameters();

        ExtraVisualTweaks();
    }

    // Helper Functions --------------------------------------------------------

    void UpdateAnimationParameters()
    {
        anim.SetFloat("Speed", Mathf.Abs(GetComponent<PlayerMove>().rb.linearVelocityX));
    }

    void ExtraVisualTweaks()
    {
        // Flip
        if (GetComponent<PlayerMove>().rb.linearVelocityX < 0) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
    }
}
