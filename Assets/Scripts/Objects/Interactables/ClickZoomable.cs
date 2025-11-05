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
    [HideInInspector] public bool wrongPasswordInput = false;

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
                //Debug.Log("clicked UI Element " + result.gameObject.name + " current order name " + passwordOrder[orderIndex].gameObjectName); ;
                Debug.Log("index " + orderIndex + " , total " + passwordOrderInput.Length.ToString());

                SwapImage(result.gameObject);

                if (result.gameObject.name != passwordOrder[orderIndex].gameObjectName)
                {
                    wrongPasswordInput = true;
                    Debug.Log("wrong password");
                }
                else if (result.gameObject == exitButton)
                {
                    //leaveCondition = true;
                    OnExit();
                }

                if (orderIndex >= passwordOrderInput.Length - 1)
                {
                    if (wrongPasswordInput == false)
                    {
                        Debug.Log("Unlocked");
                        SetAll(toActivate, true);
                        SetAll(toDeactivate, false);
                        OnExit();
                    }
                    else
                    {
                        orderIndex = 0;
                        wrongPasswordInput = false;
                        Debug.Log("Fail. Resetting");
                        for (int i = 0; i < passwordOrderInput.Length; i++)
                        {
                            SwapImage(passwordOrderInput[i]);
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

    void SwapImage(GameObject gameObject)
    {
        UnityEngine.UI.Image childImage = gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Image parentImage = gameObject.GetComponent<UnityEngine.UI.Image>();
        
        if (childImage == null || parentImage == null)
        {
            Debug.Log(parentImage.name + " has missing sprites");
        }
        else
        {
            Debug.Log("Swapping");
            Sprite temp = parentImage.sprite;
            parentImage.sprite = childImage.sprite;
            childImage.sprite = temp;
            
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

    public void OnExit() // used in InteractableObject.cs
    {
        Debug.Log("Exit");
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
        Debug.Log("Set all ");
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

}
