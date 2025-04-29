using UnityEngine;

public class TimeCoin : MonoBehaviour
{


    public AudioClip pickupSound;
    public AudioSource audioSource; // optional override, or it will use a temporary one
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.HasTimeCoin = true;
                PlaySound();
                Destroy(gameObject);
            }
        }
    }

    void PlaySound()
    {
        if (pickupSound == null) return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(pickupSound);
        }
        else
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
    }
}
