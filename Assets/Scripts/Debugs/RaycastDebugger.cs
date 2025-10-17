using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class RaycastDebugger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            Debug.Log("Raycast hits:");
            foreach (var result in results)
                Debug.Log($" - {result.gameObject.name}");
        }
    }
}
