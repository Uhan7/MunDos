using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class ObjectState : MonoBehaviour
{
    [Header("Variables To Save")]
    public bool saveActiveState = true;
    public bool savePosition = false;
    public bool saveColliderState = false;

    public ObjectSaveData CaptureState()
    {
        ObjectSaveData data = new ObjectSaveData();
        data.objectID = GetComponent<UniqueID>().ID;

        if (saveActiveState) data.isActive = gameObject.activeSelf;
        if (savePosition) data.position = transform.position;
        if (saveColliderState) data.colliderState = GetComponent<Collider2D>().enabled;

        return data;
    }

    public void RestoreState(ObjectSaveData data)
    {
        if (saveActiveState) gameObject.SetActive(data.isActive);
        if (savePosition) transform.position = data.position;
        if (saveColliderState) GetComponent<Collider2D>().enabled = data.colliderState;
    }
}
