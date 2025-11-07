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
    [SerializeField] private GameObject selected;
    [SerializeField] private bool deactivateAfter = true;

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
        orderIndex = 0;
        if (passwordOrderInput != null)
        {
            int passwordIndex = 0;
            foreach (GameObject gameObject in passwordOrderInput)
            {
                string name = gameObject.name;
                passwordOrder.Add(new PasswordElement(name, passwordIndex));

                passwordIndex++;
            }
        }

        else Debug.LogError($"{selected.name} is null");
    }
    //private IEnumerator OnMouseClick()
    //{
    //    while (leaveCondition == false)
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            HandleClick();
    //        }

    //        yield return null;
    //    }
        
    //}

    //public void RunPasswordPuzzle()
    //{
    //    if (mouseClickCoroutine == null) mouseClickCoroutine = StartCoroutine(OnMouseClick());

    //    if (leaveCondition)
    //    {
    //        OnExit();
    //    }
    //}


    // Helper Functions --------------------------------------------------------

    //private void HandleClick()
    //{
    //    PointerEventData pointerData = new PointerEventData(EventSystem.current)
    //    {
    //        position = Input.mousePosition
    //    };
    //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    //    var results = new System.Collections.Generic.List<RaycastResult>();
    //    EventSystem.current.RaycastAll(pointerData, results);
    //    if (results.Count > 0)
    //    {
    //        foreach (var result in results)
    //        {
    //            if (result.gameObject.name != passwordOrder[orderIndex].gameObjectName)
    //            {
    //                wrongPasswordInput = true;
    //            }
    //            //else if (result.gameObject == exitButton)
    //            {
    //                OnExit();
    //            }
    //        }
    //    }
    //}

    public void CheckCombination(GameObject gameObject)
    {
        Debug.Log($"CC index {orderIndex}");
        if (gameObject.name != passwordOrder[orderIndex].gameObjectName)
        {
            wrongPasswordInput = true;
        }

        if (orderIndex >= passwordOrderInput.Length - 1)
        {
           
            if (wrongPasswordInput == false)
            {
                Debug.Log($"solved");

                SetAll(toActivate, true);
                SetAll(toDeactivate, false);
                OnExit();
            }
            else
            {
                Debug.Log($"resetting");

                orderIndex = 0;
                wrongPasswordInput = false;

                SetAll(toActivateOnInvalidInteract, true);
                SetAll(toDeactivateOnInvalidInteract, false);
                ResetButtons();
            }
        }
        else
        {
            orderIndex++;
        }

    }

    private void ResetButtons()
    {
        foreach(var obj in passwordOrderInput)
        {
            UnityEngine.UI.Button button = obj.GetComponent<UnityEngine.UI.Button>();
            Debug.Log($"resetting button {button.name}");

            button.interactable = true;
        }
    }
        //private void SetLockStateParent(GameObject gameObject, bool value)
        //{

        //    if (int.TryParse(gameObject.name, out int index))
        //    {
        //        int newIndex = index - 1;

        //        if (selectedObjects[newIndex] !=  null) 
        //        {
        //            Set(selectedObjects[newIndex], !value);
        //        }
        //        else Debug.LogError($"selected object {selectedObjects[newIndex].name} is null");
        //    }
        //    Set(gameObject, value);
        //}

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
    //void Set(GameObject gameObject, bool value)
    //{
    //    if (gameObject == null) return;
    //    gameObject.SetActive(value);
    //}

    private void OnExit()
    {
        orderIndex = 0;
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
