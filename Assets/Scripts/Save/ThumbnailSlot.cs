using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThumbnailSlot : MonoBehaviour
{
    [Header("Main References")]
    [SerializeField] private List<Sprite> timelinePics = new List<Sprite>();
    [SerializeField] private int areaCount = 3;
    
    /*
     * TimelinePics should be a list of size areacount * 2
     * areaCount should be how many "areas" are in a timeline (currently set to 3 for outpost > forest > town)
     */
    public Sprite GetScreenshot(int timeline, int area)
    {
        if (timeline == 0 && area < areaCount) // past
        {
            return timelinePics[area];
        }
        else if (timeline == 1 && (area + areaCount) < timelinePics.Count)
        {
            return timelinePics[area + areaCount];
        }
        else
        {
            Debug.LogWarning("[ThumbnailSlot] : Problem with recieved {param: area} || Value: " + area);
            return timelinePics[0];
        }
    }
}
