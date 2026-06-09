using NaughtyAttributes;
using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [HideInInspector] private const string SFX_SOURCE_NAME = "SFX Source";

    [Header("References")]
    [SerializeField] private Collider2D triggerCol;
    [HideInInspector] private AudioSource sfxSource;

    [Header("Multi Interact")]
    [SerializeField] private bool onEnable;
    [SerializeField] private bool repeatable; //Can interact again after ValidItem's items's hasActivated is true
    [SerializeField] private bool resetStateAfter; //Any Activated will de deactivated
    [SerializeField] private bool deactivateAfter; //gameobejct will deactivate after
    [SerializeField] private bool isFirstState; //prevents cascading of next state
    [SerializeField] private bool isLastState; //prevents cascading of previous state

    [Header("Counter")]
    [SerializeField] private bool hasInteractionCounter;
    [ShowIf("hasInteractionCounter")][SerializeField] private int interactionCountsNeeded;
    //[ShowIf("hasInteractionCounter")][SerializeField] private bool interactionCountNeedsItem;
    [ShowIf("hasCohasInteractionCounterunter")][SerializeField] private bool persistentCounter;

    [Header("Interactions")]
    [HideIf("hasInteractionCounter")][SerializeField] ValidItem[] items;
    [ShowIf("hasInteractionCounter")][SerializeField] private GameObject[] toActivateOnCountsReached;
    [ShowIf("hasInteractionCounter")][SerializeField] private GameObject[] toDeactivateOnCountsReached;
    [ShowIf("hasInteractionCounter")][SerializeField] private AudioClip[] soundsToPlayOnInteraction;
    [ShowIf("hasInteractionCounter")][SerializeField] private AudioClip[] soundsToPlayOnInteractionsNeeded;

    [Header("Flags")]
    [ReadOnly][SerializeField] private bool stateCheck = false; //Ensures intended behaviour
    [ReadOnly][SerializeField] private bool foundEquippedItem = false;
    [ReadOnly][SerializeField] private string equippedObjectName = "";
    [ReadOnly][SerializeField] private bool hasActivatedOnce;
    [ReadOnly][SerializeField] private int interactionCount = 0;
    [ReadOnly][SerializeField] private PlayerInteract playerInteract;

    private void Awake()
    {
        if (isFirstState || isLastState) {
            stateCheck = false;
        }
        sfxSource = GameObject.Find(SFX_SOURCE_NAME).GetComponent<AudioSource>();
    }
    private void ItemInteract()
    {
        List<ValidItem> matchedGroup = new List<ValidItem>();
        GetValidItemInteractions(matchedGroup);

        if (matchedGroup != null)
        {
            foreach (var item in matchedGroup)
            {
                SetAll(item.toActivate, true);
                SetAll(item.toDeactivate, false);
            }
        }
        Reset();

    }

    private void CounterInteract()
    {
        if (interactionCount < interactionCountsNeeded)
        {
            foreach (AudioClip clip in soundsToPlayOnInteractionsNeeded) sfxSource.PlayOneShot(clip);
            SetAll(toActivateOnCountsReached, true);
            SetAll(toDeactivateOnCountsReached, false);
            Reset();
        }
        else
        {
            foreach (AudioClip clip in soundsToPlayOnInteraction) sfxSource.PlayOneShot(clip);
            interactionCount++;
        }

    }

    private void GetValidItemInteractions(List<ValidItem> matchedGroup) 
    {
        for (int i = 0; i < items.Length; i++)
        {
            var itemGroup = items[i];
            ItemData itemData = itemGroup.item.GetComponent<Item>().GetData();
            if (itemGroup == null || itemData == null)
            {
                continue;
            }

            if (itemData.itemName == equippedObjectName)
            {
                CheckCondition();

                itemGroup.hasActivated = true;
                stateCheck = false;
                matchedGroup.Add(itemGroup);
            }
        }
    }

    private void CheckCollisions()
    {
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
                if (!playerInteract)
                {
                }
                else
                {
                    equippedObjectName = playerInteract.TryGetEquippedObject().itemName;
                    //Debug.Log($"in check col | found player with item {equippedObjectName}");
                    foundEquippedItem = true;
                }
            }
        }
    }
    
    private void CheckCondition()
    {
        //if (!isFirstState && !isLastState)
        //if (!isFirstState && !isLastState && !stateCheck)
        if (((isFirstState || isLastState) && !stateCheck) ||  // if first/last states and havent checked yet
            ((!isFirstState && !isLastState) && stateCheck))
        {
            stateCheck = true;
            gameObject.SetActive(false);
        }
        else
        {
        }
    }

    public void OnEnable()
    {
        if (!onEnable) return;
        
        if (!hasInteractionCounter)
        {
            //Debug.Log("-----------new interact-----------");
            CheckCollisions();
            //Debug.Log($"Will Check || has act once {hasActivatedOnce} | found equip {foundEquippedItem} | state chk {stateCheck}");
            if (foundEquippedItem)
            {
                ItemInteract();
            }
        }
        else
        {
            CounterInteract();
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
        Debug.Log($"in reset");
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
        if (!persistentCounter){
            interactionCount = 0;
        }
        if (repeatable)
        {
            stateCheck = false;
        }
        
        equippedObjectName = "";
        foundEquippedItem = false;
        if (deactivateAfter)
        {
            gameObject.SetActive(false);
        }
    }
}
