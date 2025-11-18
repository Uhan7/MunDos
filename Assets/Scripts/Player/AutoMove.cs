using UnityEngine;
using NaughtyAttributes;

public class AutoMove : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const string PROTAG_TAG = "Protag";
    [HideInInspector] private const string NPC_TAG = "NPC Zone";
    [HideInInspector] private enum Direction { Left, Right };

    [Header("References")]
    [HideInInspector] private PlayerMove character;

    [Header("Properties")]
    [SerializeField] private Direction moveDirection;
    [SerializeField] private bool singleUse;
    [SerializeField] private bool NPCMovement;
    [SerializeField] private bool hideUI;
    [SerializeField] private string requiredName;

    private void OnEnable()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;
        col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (requiredName != "" && requiredName != col.gameObject.transform.parent.name) return;

        if (NPCMovement && col.gameObject.tag == NPC_TAG)
        {
            character = col.gameObject.GetComponentInParent<PlayerMove>();

            col.gameObject.GetComponentInParent<LockableObject>().Lock(true);
            MoveCharacter(moveDirection);
        }

        else if (!NPCMovement && col.gameObject.tag == PROTAG_TAG)
        {
            character = col.gameObject.GetComponent<PlayerMove>();

            character.gameObject.GetComponent<PlayerInteract>().SelectItem(5);
            character.gameObject.GetComponent<PlayerInteract>().ResetStates();

            MoveCharacter(moveDirection);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (requiredName != "" && requiredName != col.gameObject.transform.parent.name) return;

        if (NPCMovement && col.gameObject.tag == NPC_TAG)
        {
            character = col.gameObject.GetComponentInParent<PlayerMove>();

            col.gameObject.GetComponentInParent<LockableObject>().Lock(true);
            MoveCharacter(moveDirection);
        }

        else if (!NPCMovement && col.gameObject.tag == PROTAG_TAG)
        {
            character = col.gameObject.GetComponent<PlayerMove>();

            character.gameObject.GetComponent<PlayerInteract>().SelectItem(5);
            character.gameObject.GetComponent<PlayerInteract>().ResetStates();

            MoveCharacter(moveDirection);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (requiredName != "" && requiredName != col.gameObject.transform.parent.name) return;

        if (NPCMovement && col.gameObject.tag == NPC_TAG)
        {
            character = col.gameObject.GetComponentInParent<PlayerMove>();

            col.gameObject.GetComponentInParent<LockableObject>().Lock(false);
            StopCharacter();
        }

        else if (!NPCMovement && col.gameObject.tag == PROTAG_TAG)
        {
            character = col.gameObject.GetComponent<PlayerMove>();
            StopCharacter();
        }
    }

    // Helper Functions --------------------------------------------------------

    private void MoveCharacter(Direction moveDirection)
    {
        if (!NPCMovement && character.tag == PROTAG_TAG && !character.canInput) return;

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

        if (hideUI) HideUI(true);
    }

    private void StopCharacter()
    {
        if (!NPCMovement && character.tag == PROTAG_TAG && character.canInput) return;

        if (character.tag == PROTAG_TAG) character.canInput = true;

        character.getMoveLeftKey = false;
        character.getMoveRightKey = false;

        if (singleUse) gameObject.SetActive(false);
        else {
            if (moveDirection == Direction.Right) moveDirection = Direction.Left;
            else if (moveDirection == Direction.Left) moveDirection = Direction.Right;
        }

        if (hideUI) HideUI(false);
    }

    void HideUI(bool value)
    {
        Parameters param = new Parameters();
        param.PutExtra(ParamNames.IS_HIDING_UI, value);

        EventBroadcaster.Instance.PostEvent(EventNames.HIDE_UI, param);
    }
}
