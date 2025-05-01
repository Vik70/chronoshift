using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeFreezeSpell2 : MonoBehaviour
{
    [Header("Freeze Charge Settings")]
    public int maxCharges = 3;            // Total number of freeze charges available
    private int currentCharges;           // Charges remaining

    public UIFreezeTracker freezeTracker;

    // A list for storing selected freezable objects.
    // When an object is selected via a click, one charge is used.
    private List<TimeFreezable> selectedFreezables = new List<TimeFreezable>();

    private List<TimeFreezable> frozenFreezables = new List<TimeFreezable>();

    [Header("UI Settings")]
    // Optional: a Text element to display remaining charges.
    public Text chargesUIText;

    [Header("Sound Effects")]
    public AudioClip clickSound;
    public AudioClip freezeSound;
    public AudioClip unfreezeSound;
    private AudioSource audioSource;

    // Expose public properties for UI access
    public int CurrentCharges { get { return currentCharges; } }
    public int MaxCharges { get { return maxCharges; } }

    void Start()
    {
        // Initialize with full charges.
        currentCharges = maxCharges;
        selectedFreezables.Clear();
        frozenFreezables.Clear();
        UpdateChargeUI();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 0.1f; // You can adjust this later

    }

    void Update()
    {
        // Left-click selects an object (if there are available charges).
        if (Input.GetMouseButtonDown(0))
        {
            TrySelectObjectAtCursor();
        }

        //Toggle using q
        // Pressing F freezes all currently selected objects.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedFreezables.Count > 0)
            {
                FreezeSelectedObjects();
                UpdateTrackerIcons();                // Updates the UI icons
                freezeTracker.RefreshAllStatuses(); // Updates cyan/white status

            }
            else if(frozenFreezables.Count > 0)
            {
                UnfreezeSelectedObjects();
                UpdateTrackerIcons();                // Updates the UI icons
                freezeTracker.RefreshAllStatuses(); // Updates cyan/white status

            }
            else
            {
                Debug.Log("Nothing selected or frozen to toggle");
            }
        }

    }

    void TrySelectObjectAtCursor()
    {
        // Get the world point from the mouse position.
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null)
        {
            // Try to get the TimeFreezable component on the hit object.
            TimeFreezable freezable = hit.collider.GetComponent<TimeFreezable>();
            if (freezable != null)
            {
                // Check if this object is not already selected.
                if (!selectedFreezables.Contains(freezable))
                {
                    // Check if we have any charges left.
                    if (currentCharges > 0)
                    {

                        // Consume one charge by marking the object.
                        freezable.CastFreezeSpell();  // This method should change its color (e.g., to cyan).
                        selectedFreezables.Add(freezable);
                        currentCharges--;  // Use up one charge.
                        audioSource.PlayOneShot(clickSound);
                        Debug.Log($"Selected object: {hit.collider.name}. Freeze charge used. {currentCharges} charge(s) remaining.");
                        UpdateChargeUI();
                        UpdateTrackerIcons();

                    }
                    else
                    {
                        Debug.Log("No freeze charges left.");
                    }
                }
                else
                {
                    Debug.Log("Object already selected: " + hit.collider.name);
                }
            }
        }
    }

    void FreezeSelectedObjects()
    {
        foreach (var obj in selectedFreezables)
        {
            if (obj.CompareTag("Player"))
            {
                // Respawn player and immediately unfreeze so the blue goes away
                var cp = obj.GetComponent<CheckpointManager>();
                if (cp != null) cp.Respawn();

                // Make sure player has a TimeFreezable so we can call Unfreeze()
                obj.Unfreeze();
            }
            else
            {
                // Freeze the object and remember it for later unfreeze
                obj.Freeze();
                frozenFreezables.Add(obj);
            }
        }

        // Done with this batch of selections
        selectedFreezables.Clear();
        UpdateChargeUI();
        audioSource.PlayOneShot(freezeSound);

        Debug.Log("Applied freeze/respawn to selected objects.");
    }


    void UnfreezeSelectedObjects()
    {
        foreach (var obj in frozenFreezables)
            obj.Unfreeze();

        frozenFreezables.Clear();
        audioSource.PlayOneShot(unfreezeSound);

        UpdateTrackerIcons();                // NEW
        freezeTracker.RefreshAllStatuses();  // NEW

        Debug.Log("All frozen objects have been unfrozen.");
    }



    private void UpdateChargeUI()
    {
        if (chargesUIText != null)
            chargesUIText.text = $"Freeze Charges: {currentCharges}/{maxCharges}";
    }

    void UpdateTrackerIcons()
    {
        HashSet<TimeFreezable> all = new(selectedFreezables);
        foreach (var f in frozenFreezables)
            all.Add(f);

        freezeTracker.SetTrackedObjects(new List<TimeFreezable>(all));
    }

}
