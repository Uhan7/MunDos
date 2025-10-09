using UnityEngine;

public class AutoMove : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const string PROTAG_TAG = "Protag";

    [Header("References")]
    [SerializeField] private PlayerMove character;

    [Header("Bool Tests")]
    [SerializeField] private bool moveRight;

    private void OnTriggerEnter2D(Collider2D col)
    {
        // This line is temporary cause other ppl need to auto move as well
        if (col.gameObject.tag == PROTAG_TAG)
        {
            // This part is also obvs temporary since this literally just references the character
            character.canInput = false;

            MoveCharacter('R');
        }
    }

    private void Update()
    {
        
    }

    // Helper Functions --------------------------------------------------------

    private void MoveCharacter(char direction)
    {
        if (direction == 'R')
        {
            character.getMoveLeftKey = false;
            character.getMoveRightKey = true;
        }
        else if (direction == 'L')
        {
            character.getMoveLeftKey = true;
            character.getMoveRightKey = false;
        }
        else Debug.Log("Not a direction");
    }
}
