using UnityEngine;
using NaughtyAttributes;

public class TempMove : MonoBehaviour
{
    [ReadOnly, SerializeField, Tooltip("This is just a temporary script for the stuff in Town to move to the right")] private string hoverHere;
}
