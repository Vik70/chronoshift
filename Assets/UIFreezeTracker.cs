using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFreezeTracker : MonoBehaviour
{
    public GameObject iconPrefab;
    public Transform iconParent;

    private Dictionary<TimeFreezable, Image> trackedIcons = new();

    public void SetTrackedObjects(List<TimeFreezable> objects)
    {
        // Clear old icons
        foreach (Transform child in iconParent)
        {
            Destroy(child.gameObject);
        }
        trackedIcons.Clear();

        foreach (var obj in objects)
        {
            GameObject icon = Instantiate(iconPrefab, iconParent);
            Image image = icon.GetComponent<Image>();

            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                image.sprite = sr.sprite;
                image.preserveAspect = true;
                image.color = obj.IsFrozen() ? Color.cyan : Color.white;
            }

            trackedIcons[obj] = image;
        }
    }

    public void RefreshAllStatuses()
    {
        foreach (var pair in trackedIcons)
        {
            if (pair.Key != null && pair.Value != null)
                pair.Value.color = pair.Key.IsFrozen() ? Color.cyan : Color.white;
        }
    }
}
