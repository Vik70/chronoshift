using UnityEngine;

public class EndLevelDoor : MonoBehaviour
{
    [Header("Transition")]
    public LevelTransitionManager transitionManager;  // Drag this in Inspector
    public Animator doorAnimator;
    public string openTrigger = "open";

    private bool hasActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasActivated) return;
        if (!collision.CompareTag("Player") || transitionManager == null) return;

        hasActivated = true;
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(openTrigger);
        }

        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.enabled = false;
        }

        // Check inventory
        var inventory = collision.GetComponent<PlayerInventory>();
        bool hasTimeCoin = inventory != null && inventory.HasTimeCoin;

        // Start transition with result
        transitionManager.StartTransition(hasTimeCoin);
    }
}
