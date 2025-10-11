using UnityEngine;

public class AutoMove : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const string PROTAG_TAG = "Protag";
    [HideInInspector] private const string NPC_TAG = "NPC";
    [HideInInspector] private enum Direction { Left, Right };

    [Header("References")]
    [HideInInspector] private PlayerMove character;

    [Header("Properties")]
    [SerializeField] private Direction moveDirection;
    [SerializeField] private bool stopper;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == PROTAG_TAG || col.gameObject.tag == NPC_TAG)
        {
            character = col.gameObject.GetComponent<PlayerMove>();
            character.canInput = false;

            MoveCharacter(moveDirection);
        }
    }

    private void Update()
    {
        
    }

    // Helper Functions --------------------------------------------------------

    private void MoveCharacter(Direction moveDirection)
    {
        if (moveDirection == Direction.Right)
        {
            character.getMoveLeftKey = false;
            character.getMoveRightKey = true;
        }
        else if (moveDirection == Direction.Left)
        {
            character.getMoveLeftKey = true;
            character.getMoveRightKey = false;
        }
    }
}
