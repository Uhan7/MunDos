using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using System.Collections;
// using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using NaughtyAttributes;

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
    [SerializeField] private GameObject[] passwordOrderInput;
    [HideIf("isSingleEnvi")][SerializeField] private bool hasActivatedState;
    [SerializeField] private bool isSingleEnvi;
    [ShowIf("isSingleEnvi")][SerializeField] private GameObject exitButton;
    [ShowIf("isSingleEnvi")][SerializeField] private GameObject zoomEnviBackdrop;
    [ShowIf("isSingleEnvi")][SerializeField] private GameObject zoomEnviImage;
    [SerializeField] private bool deactivateAfter;
    [SerializeField] private bool disableInteractionAfter;
    [SerializeField] private bool toActivateOnAnyClick = false;

    [HideInInspector] private List<PasswordElement> passwordOrder = new List<PasswordElement>();
    [HideInInspector] private List<string> inputOrder = new List<string>();
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
    [HideInInspector] public bool hasFirstPress = false;


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
    }

    private void Update()
    {
        //if (Input.GetKeyDown(deactivateKey))
        //{
        //    if (hasFirstPress)
        //    {
        //        if (isSingleEnvi) ResetButtons();
        //        OnExit();
        //        return;
        //    }
        //    else
        //    {
        //        hasFirstPress = true;
        //    }

        //}
    }

    // Helper Functions --------------------------------------------------------

    //gameobject is a reference to the button itself
    //Button interact and checking is done here
    public void CheckCombination(GameObject gameObject)
    {
        //Debug.Log($"CZ: checking {gameObject.name}, matching order {gameObject.name == passwordOrder[orderIndex].gameObjectName}");
        if (toActivateOnAnyClick)
        {
            if (isSingleEnvi) ResetButtons();
            SetAll(toActivate, true);
            SetAll(toDeactivate, false);
            OnExit();
        }
        if (inputOrder.Contains(gameObject.name))
        {
            return;
        }
        inputOrder.Add(gameObject.name);

        if (!isSingleEnvi)
        {
            SetInteractableObjectOutline(gameObject, true);
            if (hasActivatedState)
            {
                SetActiveState(gameObject, true);
            }
        }
        if (gameObject.name != passwordOrder[orderIndex].gameObjectName)
        {
            wrongPasswordInput = true;
        }

        //ebug.Log($"order Index {orderIndex}, passwordInput length {passwordOrderInput.Length - 1}");
        if (orderIndex >= passwordOrderInput.Length - 1)
        {
           
            if (wrongPasswordInput == false)
            {
                //Debug.Log("CZ: Correctly solved puzzle");
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
                inputOrder.Clear();

                //Debug.Log("CZ: Fail puzzle. Resetting");
                if (isSingleEnvi) ResetButtons();
                else
                {
                    foreach (var obj in passwordOrderInput)
                    {
                        SetInteractableObjectOutline(obj, false);
                        if (hasActivatedState) SetActiveState(obj, false);
                    }
                }
            }
        }
        else
        {
            orderIndex++;
            if (isSingleEnvi) DeactivateButton(gameObject);
        }

    }

    public void DeactivateButton(GameObject obj)
    {
        UnityEngine.UI.Button button = obj.GetComponent<UnityEngine.UI.Button>();
        button.interactable = false;
    }
    private void ResetButtons()
    {
        foreach(var obj in passwordOrderInput)
        {
            UnityEngine.UI.Button button = obj.GetComponent<UnityEngine.UI.Button>();
            //CanvasGroup canvasGroup = button.GetComponentInParent<CanvasGroup>();
            //if (canvasGroup != null)
            //{
            //    canvasGroup.interactable = true;
            //}
            button.OnPointerExit(null);
            button.interactable = true;
            Debug.Log($"resetting {obj.name} button {button.name} make interac {button.interactable}");

            //ExecuteEvents.Execute(button.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.deselectHandler);
        }
        Canvas.ForceUpdateCanvases();
    }
    //
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

    void SetActiveState(GameObject obj, bool value)
    {
        Transform childTransformActive = obj.transform.Find("Active");
        Transform childTransformSymbol = obj.transform.Find("Symbol");

        if (childTransformActive == null) Debug.LogError($"{childTransformActive.name}'s active is null");
        else childTransformActive.gameObject.SetActive(value);

        
        if (childTransformSymbol == null) Debug.LogError($"{childTransformSymbol.name}'s active is null");
        else childTransformSymbol.gameObject.SetActive(!value);
    }


    private void OnExit()
    {
        orderIndex = 0;
        if (isSingleEnvi)
        {
            zoomEnviBackdrop.SetActive(false);
            zoomEnviImage.SetActive(false);
        }

        if (disableInteractionAfter)
        {
            foreach (var obj in passwordOrderInput)
            {
                SetInteractableObjectOutline(obj, false);
                if (obj.GetComponent<BoxCollider2D>() != null)
                {
                    obj.GetComponent<BoxCollider2D>().enabled = false;
                    
                }
                if (obj.GetComponent<InteractableObject>() != null)
                {
                    obj.GetComponent<InteractableObject>().enabled = false;
                }
            }
        }
        hasFirstPress = false;

        leaveCondition = false;
        if (deactivateAfter)
        {
            this.gameObject.SetActive(false);
        }
    }
    
    void SetInteractableObjectOutline(GameObject obj, bool value)
    {

        if (obj == null) Debug.LogError($"obj {obj.name} is null");
        SpriteRenderer objSpriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (objSpriteRenderer == null) return;//Debug.LogError($"Sprite render {objSpriteRenderer.name} is null");
        Sprite objOutlinedSprite = obj.GetComponent<InteractableObject>().outlinedSprite;
        Sprite objNormalSprite = obj.GetComponent<InteractableObject>().normalSprite;

        if (value == true)
        {
            if (objOutlinedSprite != objNormalSprite) objSpriteRenderer.sprite = objOutlinedSprite;
            else obj.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f, 1);
        }
        else
        {
            if (objOutlinedSprite != objNormalSprite) objSpriteRenderer.sprite = objNormalSprite;
            else obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    IEnumerator WaitForTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
