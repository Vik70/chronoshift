using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DoorUnlock : MonoBehaviour
{
    [Header("Optional: Animation")]
    public Animator doorAnimator; // if you have an "open" animation
    public string openTrigger = "open";

    [Header("Or just disable")]
    public GameObject doorSprite; // assign the sprite or root of the door

    private bool isOpen = false;

    public AudioClip unlockSound;
    public AudioSource audioSource; // assign for central door sound or use PlayClipAtPoint


    void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpen || !other.CompareTag("Player")) return;

        var inv = other.GetComponent<PlayerInventory>();
        if (inv != null && inv.UseKey())
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        PlaySound();
        Debug.Log("Door opened!");

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(openTrigger);
        }
        else if (doorSprite != null)
        {
            doorSprite.SetActive(false); // hide the door so player can pass
        }
        else
        {
            // If no animator or sprite specified, just disable this collider:
            GetComponent<Collider2D>().enabled = false;
        }
        BoxCollider2D solidCollider = GetComponent<BoxCollider2D>();

        if (solidCollider != null)
        {
            solidCollider.enabled = false; // allow the player to walk through
        }
    }

    void PlaySound()
    {
        if (unlockSound == null) return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(unlockSound);
        }
        else
        {
            AudioSource.PlayClipAtPoint(unlockSound, transform.position);
        }
    }
}
