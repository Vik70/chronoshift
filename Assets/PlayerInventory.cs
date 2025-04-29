using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("UI")]
    public Image keyIcon;    // Assign in inspector: a UI Image of the key

    public bool HasKey { get; private set; } = false;
    public bool HasTimeCoin { get; set; } = false;


    void Start()
    {
        // Hide the key icon until we pick up a key
        if (keyIcon != null) keyIcon.enabled = false;
    }

    public void GrantKey()
    {
        HasKey = true;
        Debug.Log("Player Received the key!");
        //Show key UI
    }

    /// <summary>
    /// Call this to grant the player a key.
    /// </summary>
    public void PickUpKey()
    {
        HasKey = true;
        if (keyIcon != null) keyIcon.enabled = true;
        Debug.Log("Key picked up!");
    }

    /// <summary>
    /// Use (consume) the key.
    /// </summary>
    public bool UseKey()
    {
        if (!HasKey) return false;
        HasKey = false;
        if (keyIcon != null) keyIcon.enabled = false;
        return true;
    }
}
