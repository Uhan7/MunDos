using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Collections.Generic;
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
    [SerializeField] private bool isSingleEnvi;
    [ShowIf("isSingleEnvi")][SerializeField] private GameObject exitButton;
    [ShowIf("isSingleEnvi")][SerializeField] private GameObject zoomEnviBackdrop;
    [ShowIf("isSingleEnvi")][SerializeField] private GameObject zoomEnviImage;
    [SerializeField] private bool deactivateAfter;
    [SerializeField] private bool disableInteractionAfter;

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


    // Helper Functions --------------------------------------------------------

    //gameobject is a reference to the button itself
    //Button interact and checking is done here
    public void CheckCombination(GameObject gameObject)
    {
        Debug.Log($"CZ: checking {gameObject.name}, matching order {gameObject.name == passwordOrder[orderIndex].gameObjectName}");
        if (inputOrder.Contains(gameObject.name))
        {
            return;
        }
        inputOrder.Add(gameObject.name);
        if (!isSingleEnvi)
        {
            SetInteractableObjectOutline(gameObject, true);
        }
        
        if (gameObject.name != passwordOrder[orderIndex].gameObjectName)
        {
            wrongPasswordInput = true;
        }
        //Debug.Log($"order Index {orderIndex}, passwordInput length {passwordOrderInput.Length - 1}");
        if (orderIndex >= passwordOrderInput.Length - 1)
        {
           
            if (wrongPasswordInput == false)
            {
                Debug.Log("CZ: Correctly solved puzzle");
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

                if (isSingleEnvi) ResetButtons();
                else
                {
                    foreach (var obj in passwordOrderInput)
                    {
                        SetInteractableObjectOutline(obj, false);
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
            button.interactable = true;
        }
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
    //void Set(GameObject gameObject, bool value)
    //{
    //    if (gameObject == null) return;
    //    gameObject.SetActive(value);
    //}

    private void OnExit()
    {
        orderIndex = 0;
        if (isSingleEnvi)
        {
            zoomEnviBackdrop.SetActive(false);
            zoomEnviImage.SetActive(false);
        }
        
        leaveCondition = false;
        if (deactivateAfter)
        {
            this.gameObject.SetActive(false);
        }

        if (disableInteractionAfter)
        {
            
            foreach (var obj in passwordOrderInput)
            {
                SetInteractableObjectOutline(obj, false);
                obj.GetComponent<BoxCollider2D>().enabled = false;
                obj.GetComponent<InteractableObject>().enabled = false;
            }
        }
    }
    
    void SetInteractableObjectOutline(GameObject obj, bool var)
    {

        if (obj == null) Debug.LogError($"obj {obj.name} is null");
        SpriteRenderer objSpriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (objSpriteRenderer == null) Debug.LogError($"Sprite render {objSpriteRenderer.name} is null");
        Sprite objOutlinedSprite = obj.GetComponent<InteractableObject>().outlinedSprite;
        Sprite objNormalSprite = obj.GetComponent<InteractableObject>().normalSprite;

        if (var == true)
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
