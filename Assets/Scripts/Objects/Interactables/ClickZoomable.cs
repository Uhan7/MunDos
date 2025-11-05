using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;

[System.Serializable]
public class PasswordElement
{
    public PasswordElement(string gameObjectName, int order)
    {
        this.gameObjectName = gameObjectName;
        this.order = order;
    }
    public string gameObjectName;
    public int order;

}
public class ClickZoomable : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private string CLICKABLE_CANVAS = "CanvasClickable";

    [Header("Keybinds")]
    [SerializeField] private KeyCode deactivateKey;

    [Header("Components")]
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject zoomEnviBackdrop;
    [SerializeField] private GameObject zoomEnviImage;
    [SerializeField] private GameObject[] passwordOrderInput;
    [SerializeField] private bool deactivateAfter = true;

    [SerializeField] private GameObject selected;
    [SerializeField] private GameObject[] selectedObjects;
    [HideInInspector] private List<PasswordElement> passwordOrder = new List<PasswordElement>();
    
    [HideInInspector] private int orderIndex = 0;

    [Header("Gameobjects")]
    [SerializeField] private GameObject[] toActivate;
    [SerializeField] private GameObject[] toDeactivate;
    [SerializeField] private GameObject[] toActivateOnInvalidInteract;
    [SerializeField] private GameObject[] toDeactivateOnInvalidInteract;

    [Header("Flags")]
    [HideInInspector] public bool hasElements = false;
    [HideInInspector] public bool leaveCondition = false;
    [HideInInspector] public bool wrongPasswordInput = false;

    private Coroutine mouseClickCoroutine;

    private void Start()
    {
        if (passwordOrderInput != null)
        {
            int passwordIndex = 0;
            foreach (GameObject gameObject in passwordOrderInput)
            {
                string name = gameObject.name;
                passwordOrder.Add(new PasswordElement(name, passwordIndex));

                passwordIndex++;
            }

            foreach(GameObject gameObject in selectedObjects)
            {
                UnityEngine.UI.Image image = gameObject.GetComponent<UnityEngine.UI.Image>();
                image.raycastTarget = false;
            }
        }
        if (selected != null)
        {
            selectedObjects = new GameObject[selected.transform.childCount];
            int selectedObjIndex = 0;
            foreach (Transform child in selected.transform)
            {
                selectedObjects[selectedObjIndex] = child.gameObject;
                selectedObjIndex++;
            }
        }
        else Debug.LogError($"{selected.name} is null");
    }
    private IEnumerator OnMouseClick()
    {
        while (leaveCondition == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleClick();
            }

            yield return null;
        }
        
    }

    public void RunPasswordPuzzle()
    {
        if (mouseClickCoroutine == null) mouseClickCoroutine = StartCoroutine(OnMouseClick());

        if (leaveCondition)
        {
            OnExit();
        }
    }


    // Helper Functions --------------------------------------------------------

    private void HandleClick()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {

            foreach (var result in results)
            {
                if (!result.gameObject.CompareTag(CLICKABLE_CANVAS)) continue;

                SetLockStateParent(result.gameObject, false);

                if (result.gameObject.name != passwordOrder[orderIndex].gameObjectName)
                {
                    wrongPasswordInput = true;
                }
                else if (result.gameObject == exitButton)
                {
                    OnExit();
                }

                if (orderIndex >= passwordOrderInput.Length - 1)
                {
                    if (wrongPasswordInput == false)
                    {
                        SetAll(toActivate, true);
                        SetAll(toDeactivate, false);
                        OnExit();
                    }
                    else
                    {
                        orderIndex = 0;
                        wrongPasswordInput = false;

                        SetAll(toActivateOnInvalidInteract, true);
                        SetAll(toDeactivateOnInvalidInteract, false);

                        foreach (GameObject gameObject in passwordOrderInput)
                        {
                            SetLockStateParent(gameObject, true);
                        }
                    }  
                }
                else
                {
                    orderIndex++;
                }
            }
        }
    }

    private void SetLockStateParent(GameObject gameObject, bool value)
    {

        if (int.TryParse(gameObject.name, out int index))
        {
            int newIndex = index - 1;

            if (selectedObjects[newIndex] !=  null) 
            {
                Set(selectedObjects[newIndex], !value);
            }
            else Debug.LogError($"selected object {selectedObjects[newIndex].name} is null");
        }
        Set(gameObject, value);
    }
    
    void SetAll(GameObject[] objects, bool value)
    {
        if (objects == null || objects.Length == 0) return;
        foreach (GameObject obj in objects)
        {
            if (obj == null)
            {
                continue;
            }
            obj.SetActive(value);
        }
    }
    void Set(GameObject gameObject, bool value)
    {
        if (gameObject == null) return;
        gameObject.SetActive(value);
    }

    private void OnExit()
    {
        StopCoroutine(mouseClickCoroutine);
        zoomEnviBackdrop.SetActive(false);
        zoomEnviImage.SetActive(false);
        leaveCondition = false;
        if (deactivateAfter)
        {
            this.gameObject.SetActive(false);
        }
    }
}
