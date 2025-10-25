using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Collections;

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

    [HideInInspector] private List<PasswordElement> passwordOrder = new List<PasswordElement>();
    [HideInInspector] private int orderIndex = 0;

    [Header("Gameobjects")]
    [SerializeField] private GameObject[] toActivate;
    [SerializeField] private GameObject[] toDeactivate;

    [Header("Flags")]
    [HideInInspector] public bool hasElements = false;
    [HideInInspector] public bool leaveCondition = false;

    private Coroutine mouseClickCoroutine;

    private void Start()
    {
        if (passwordOrderInput != null)
        {
            int index = 0;
            foreach (GameObject gameObject in passwordOrderInput)
            {
                string name = gameObject.name;
                passwordOrder.Add(new PasswordElement(name, index));
                index++;
            }
        }
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
                if (!result.gameObject.CompareTag(CLICKABLE_CANVAS))
                {
                    continue;
                }
                Debug.Log("clicked UI Element " + result.gameObject.name + " current order name " + passwordOrder[orderIndex].gameObjectName); ;

                if (result.gameObject.name == passwordOrder[orderIndex].gameObjectName)
                {
                    orderIndex++;
                    if (orderIndex == passwordOrder.Count)
                    {
                        Debug.Log("Unlocked");
                        SetAll(toActivate, true);
                        SetAll(toDeactivate, false);
                        OnExit();
                    }
                }
                else if (result.gameObject == exitButton)
                {
                    //leaveCondition = true;
                    OnExit();
                }
                else
                {
                    orderIndex = 0;
                    Debug.Log("Fail. Resetting");
                }
            }
        }
    }
    public void RunPasswordPuzzle()
    {
        Debug.Log("In Run pass func");
        if (mouseClickCoroutine == null) mouseClickCoroutine = StartCoroutine(OnMouseClick());

        if (leaveCondition)
        {
            OnExit();
        }
    }

    public void OnExit() // used in InteractableObject.cs
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

    void SetAll(GameObject[] objects, bool value)
    {
        if (objects == null || objects.Length == 0) return;
        foreach (GameObject obj in objects) obj.SetActive(value);
    }

}
