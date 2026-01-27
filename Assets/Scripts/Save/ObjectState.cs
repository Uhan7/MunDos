using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class ObjectState : MonoBehaviour
{
    [Header("Variables To Save")]
    public bool saveActiveState = true;
    public bool savePosition = false;

    public ObjectSaveData CaptureState()
    {
        ObjectSaveData data = new ObjectSaveData();
        data.objectID = GetComponent<UniqueID>().ID;

        if (saveActiveState) data.isActive = gameObject.activeSelf;
        if (savePosition)
        {
            data.position = transform.position;
            print(data.position);
        }
        return data;
    }

    public void RestoreState(ObjectSaveData data)
    {
        if (savePosition) transform.position = data.position;
        if (saveActiveState) gameObject.SetActive(data.isActive);
    }
}
