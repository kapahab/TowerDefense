using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EntityTrackerUI : MonoBehaviour
{
    [Header("Tracking Settings")]
    [SerializeField] private List<Image> trackerCircles = new List<Image>();
    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private Color inactiveColor = Color.gray;

    /// <summary>
    /// Updates the visual state of the tracker circles based on the number of active entities.
    /// </summary>
    /// <param name="activeCount">The current number of active entities to track.</param>
    public void UpdateTrackers(int activeCount)
    {
        for (int i = 0; i < trackerCircles.Count; i++)
        {
            if (trackerCircles[i] != null)
            {
                // Toggle color based on whether the index is within the active count
                trackerCircles[i].color = (i < activeCount) ? activeColor : inactiveColor;
            }
        }
    }
}
