using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Unity.VisualScripting;
using System;


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
    [SerializeField] private bool onlyActivateOnce;
    [SerializeField] private bool resetStateAfter;
    [SerializeField] private bool deactivateAfter;
    [SerializeField] private bool hasCounter;

    [Header("Properties")]
    [SerializeField] public bool hasActivatedOnce;


    [SerializeField] private Collider2D triggerCol;
    [SerializeField] private PlayerInteract playerInteract;
    [HideInInspector] public int currentInteractions = 0;

    public void Interact()
    {
        foreach (var itemGroup in items)
        {
            ItemData itemData = itemGroup.item.GetComponent<ItemData>();
            if (itemGroup == null || itemData == null) continue;
            if (itemData.itemName == playerInteract.TryGetEquippedObject().itemName)
            {
                Debug.Log($"item names {itemData.itemName} | {playerInteract.TryGetEquippedObject().itemName} match");
                SetAll(itemGroup.toActivate, true);
                SetAll(itemGroup.toDeactivate, false);
                itemGroup.hasActivated = true;
            }
            else
            {
                Debug.Log($"item names {itemData.itemName} | {playerInteract.TryGetEquippedObject().itemName} dont match");
            }
            
        }

    }

    private void Awake()
    {
        InitializeCache();
    }

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.gameObject.tag != PROTAG_TAG) return;
    //}

    //private void OnTriggerExit2D(Collider2D col)
    //{
    //    if (col.gameObject.tag != PROTAG_TAG) return;
    //}

    private void InitializeCache()
    {
        //triggerCol = GetComponent<Collider2D>();

    }

    public void OnEnable()
    {
        if (!onEnable) return;

        Collider2D[] results = new Collider2D[10];

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        int count = triggerCol.Overlap(filter, results);
        for (int i = 0; i < count; i++)
        {
            Collider2D col = results[i];
            if (col.CompareTag(PROTAG_TAG))
            {
                Debug.Log("found player");
                playerInteract = col.GetComponent<PlayerInteract>();
                if (!playerInteract)
                {
                    Debug.LogWarning("no player interact");
                }
                else
                {
                    Debug.Log($"found player with item {playerInteract.TryGetEquippedObject().itemName}");
                }
            }
        }
        Interact();
        Reset();
    }
    
    void SetAll(GameObject[] objects, bool value)
    {
        if (objects == null) return;

        foreach (GameObject obj in objects)
        {
            //Debug.Log($"activating {obj.name}");
            if (obj == null) continue;
            obj.SetActive(value);
        }
    }

    private void Reset()
    {
        if (deactivateAfter)
        {
            gameObject.SetActive(false);
        }
        if (resetStateAfter){
            foreach (var itemGroup in items)
            {
                if (itemGroup.hasActivated)
                {
                    SetAll(itemGroup.toActivate, false);
                    SetAll(itemGroup.toDeactivate, true);
                }
                if (!onlyActivateOnce && itemGroup.hasActivated)
                {
                    itemGroup.hasActivated = false;
                }
            }
            currentInteractions = 0;
        }

    }
}
