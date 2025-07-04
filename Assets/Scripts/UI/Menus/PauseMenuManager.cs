using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    private Animator animator;
    private bool active = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Active", active);
    }

    public void togglePauseScreen()
    {
        active = !active;
    }
}
