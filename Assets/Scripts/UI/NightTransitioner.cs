using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class NightTransitioner : MonoBehaviour
{
    [Header("Colors Used")]
    [SerializeField] Color darkGrey;
    [SerializeField] Color lightGrey;

    [Header("Past References")]
    [SerializeField] List<GameObject> past_DarkGrey = new List<GameObject>();
    [SerializeField] List<GameObject> past_LightGrey = new List<GameObject>();

    [Header("Present References")]
    [SerializeField] List<GameObject> present_DarkGrey = new List<GameObject>();
    [SerializeField] List<GameObject> present_LightGrey = new List<GameObject>();

    [Header("All lights")]
    [SerializeField] List<GameObject> lights = new List<GameObject>();

    /*
     * Just triggers all the night visuals upon getting activated
     */

    private void OnEnable()
    {
        foreach (GameObject go in past_DarkGrey)
        {
            go.GetComponent<SpriteRenderer>().color = darkGrey;
        }
        foreach (GameObject go in past_LightGrey)
        {
            go.GetComponent<SpriteRenderer>().color = lightGrey;
        }
        foreach (GameObject go in present_DarkGrey)
        {
            go.GetComponent<SpriteRenderer>().color = darkGrey;
        }
        foreach (GameObject go in present_LightGrey)
        {
            go.GetComponent<SpriteRenderer>().color = lightGrey;
        }
        foreach (GameObject go in lights)
        {
            go.SetActive(true);
        }
    }
}
