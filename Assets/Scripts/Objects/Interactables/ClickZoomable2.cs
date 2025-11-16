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
public class PasswordElement2
{
    public PasswordElement2(string gameObjectName, int order)
    {
        this.gameObjectName = gameObjectName;
        this.order = order;
    }
    public string gameObjectName;
    public int order;
}
public class ClickZoomable2 : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private string CLICKABLE_CANVAS = "CanvasClickable";

    [Header("Keybinds")]
    [SerializeField] private KeyCode deactivateKey;

    [Header("Components")]
    [SerializeField] private GameObject[] passwordOrderInput;
    //[SerializeField] private bool isSingleEnvi;
    //[ShowIf("isSingleEnvi")][SerializeField] private GameObject exitButton;
    //[ShowIf("isSingleEnvi")][SerializeField] private GameObject zoomEnviBackdrop;
    //[ShowIf("isSingleEnvi")][SerializeField] private GameObject zoomEnviImage;
    [SerializeField] private bool deactivateAfter = true;

    [HideInInspector] private List<PasswordElement2> passwordOrder = new List<PasswordElement2>();
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
                passwordOrder.Add(new PasswordElement2(name, passwordIndex));
                Debug.Log($"Added {name} at index {passwordIndex}");
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
            Debug.Log($"soft wrong input");
        }
        Debug.Log($"order Index of {gameObject.name}: {orderIndex}, passwordInput length {passwordOrderInput.Length - 1}");
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
                //ResetButtons();
            }
        }
        else
        {
            orderIndex++;
            //DeactivateButton(gameObject);
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

    private void OnExit()
    {
        orderIndex = 0;
        //zoomEnviBackdrop.SetActive(false);
        //zoomEnviImage.SetActive(false);
        leaveCondition = false;
        if (deactivateAfter)
        {
            this.gameObject.SetActive(false);
        }
    }
}
