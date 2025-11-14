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
    [SerializeField] private bool isSingleEnvi;
    [ShowIf("isSingleEnvi")][SerializeField] private GameObject exitButton;
    [ShowIf("isSingleEnvi")][SerializeField] private GameObject zoomEnviBackdrop;
    [ShowIf("isSingleEnvi")][SerializeField] private GameObject zoomEnviImage;
    [SerializeField] private bool deactivateAfter = true;
    [SerializeField] private GameObject[] passwordOrderInput;

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
        
        if (gameObject.name != passwordOrder[orderIndex].gameObjectName)
        {
            wrongPasswordInput = true;
        }
        Debug.Log($"order Index {orderIndex}, passwordInput length {passwordOrderInput.Length - 1}");
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
            DeactivateButton(gameObject);
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
        zoomEnviBackdrop.SetActive(false);
        zoomEnviImage.SetActive(false);
        leaveCondition = false;
        if (deactivateAfter)
        {
            this.gameObject.SetActive(false);
        }
    }
}
