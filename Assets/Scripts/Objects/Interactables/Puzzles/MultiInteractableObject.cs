using NaughtyAttributes;
using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


// TODO: add to ObjectState
[System.Serializable]
public class ValidItem
{
    [SerializeField] public GameObject item;
    [SerializeField] public GameObject[] toActivate;
    [SerializeField] public GameObject[] toDeactivate;
    [SerializeField] public bool hasActivated = false;
}

public class MultiInteractableObject : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private string PROTAG_TAG = "Protag";

    [Header("Interactions")]
    [SerializeField] ValidItem[] items;

    [Header("Properties")]
    [SerializeField] private bool onEnable;
    [SerializeField] private bool repeatable;
    [SerializeField] private bool resetStateAfter;
    [SerializeField] private bool deactivateAfter;
    [SerializeField] private bool hasCounter;
    [SerializeField] private bool isFirstState; //prevents cascading of next state
    [SerializeField] private bool isLastState; //prevents cascading of next state

    [Header("Properties")]
    [SerializeField] public bool hasActivatedOnce;


    [SerializeField] private Collider2D triggerCol;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] public int currentInteractions = 0;
    [SerializeField] private string equippedObjectName = "";
    [SerializeField] private bool foundEquippedItem = false;
    [SerializeField] private bool stateCheck = false;

    private void Awake()
    {
        //if (!(isFirstState && isLastState))
        //{
        //    Debug.Log("become true a");
        //    stateCheck = true;
        //}
        if (isFirstState || isLastState) {
            stateCheck = false;
        }
    }
    private void Interact()
    {
        Debug.Log("in interact");
        bool firstFound = false;
        foreach (var itemGroup in items)
        {
            ItemData itemData = itemGroup.item.GetComponent<Item>().GetData();
            if (itemGroup == null || itemData == null)
            {
                continue;
            }
            if (itemData.itemName == equippedObjectName)
            {
                Debug.Log($"item names {itemData.itemName} | {equippedObjectName} match");
                itemGroup.hasActivated = true;
                Debug.Log("become true b");
                firstFound = true;
                stateCheck = false;

                SetAll(itemGroup.toActivate, true);
                SetAll(itemGroup.toDeactivate, false);
            }
            
        }
        if (firstFound)
        {
            Reset();
        } 
        else
        {
            foundEquippedItem = false;
            equippedObjectName = "";
        }

    }

    private void CheckCollisions()
    {
        Debug.Log("in check col");
        Collider2D[] results = new Collider2D[10];

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        int count = triggerCol.Overlap(filter, results);
        for (int i = 0; i < count; i++)
        {
            Collider2D col = results[i];
            if (col.CompareTag(PROTAG_TAG))
            {
                playerInteract = col.GetComponent<PlayerInteract>();
                Debug.Log($"eqiuipeed item {equippedObjectName}");
                if (!playerInteract)
                {
                }
                else
                {
                    equippedObjectName = playerInteract.TryGetEquippedObject().itemName;
                    Debug.Log($"found player with item {equippedObjectName}");
                    foundEquippedItem = true;
                }
            }
        }
    }
    


    public void OnEnable()
    {
        if (!onEnable) return;
        //if (!isFirstState && !isLastState)
        //if (!isFirstState && !isLastState && !stateCheck)
        if (((isFirstState || isLastState) && !stateCheck) || 
            ((!isFirstState && !isLastState) && stateCheck))
        {
            Debug.Log("in if");
            stateCheck = true;
            gameObject.SetActive(false);
        }

        //StartCoroutine(OnEnableCoroutine());
        CheckCollisions();
        if (foundEquippedItem)
        {
            Interact();
        }
    }

    private IEnumerator OnEnableCoroutine()
    {
        yield return new WaitForSeconds(0.12f);
        CheckCollisions();
        if (foundEquippedItem)
        {
            Interact();
        }
        else
        {
        }
    }
    
    void SetAll(GameObject[] objects, bool value)
    {
        if (objects == null) return;

        foreach (GameObject obj in objects)
        {
            if (obj == null) continue;
            obj.SetActive(value);
                
        }
    }

    private void Reset()
    {
        if (deactivateAfter)
        {
            gameObject.SetActive(false);
            foundEquippedItem = false;
        }
        foreach (var itemGroup in items)
        {
            if (resetStateAfter && itemGroup.hasActivated)
            {
                SetAll(itemGroup.toActivate, false);
                SetAll(itemGroup.toDeactivate, true);
            }
            if (repeatable && itemGroup.hasActivated)
            {
                itemGroup.hasActivated = false;
            }
        }
        if (resetStateAfter){
            currentInteractions = 0;
        }
        if (repeatable)
        {
            stateCheck = false;
        }
        if (deactivateAfter)
        {
            gameObject.SetActive(false);
        }
    }
}
