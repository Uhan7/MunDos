using UnityEngine;
using NaughtyAttributes;

public class AutoMove : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const string PROTAG_TAG = "Protag";
    [HideInInspector] private const string NPC_TAG = "NPC";
    [HideInInspector] private enum Direction { Left, Right };

    [Header("References")]
    [HideInInspector] private PlayerMove character;

    [Header("Properties")]
    [HideIf("stopper")] [SerializeField] private Direction moveDirection;
    [SerializeField] private bool stopper;
    [SerializeField] private bool NPCMovement;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (NPCMovement && col.gameObject.tag == NPC_TAG)
        {
            character = col.gameObject.GetComponent<PlayerMove>();

            if (stopper)
            {
                StopCharacter();
            }
            else
            {
                MoveCharacter(moveDirection);
            }
        }

        else if (!NPCMovement && col.gameObject.tag == PROTAG_TAG)
        {
            character = col.gameObject.GetComponent<PlayerMove>();

            if (stopper) StopCharacter();
            else MoveCharacter(moveDirection);
        }
    }

    // Helper Functions --------------------------------------------------------

    private void MoveCharacter(Direction moveDirection)
    {
        character.canInput = false;

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

    private void StopCharacter()
    {
        if (character.tag == PROTAG_TAG) character.canInput = true;

        character.getMoveLeftKey = false;
        character.getMoveRightKey = false;

        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
