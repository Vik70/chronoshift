using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KeyPickup : MonoBehaviour
{
    public AudioClip pickupSound;
    public AudioSource audioSource; // optional override, or it will use a temporary one

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            var inv = other.GetComponent<PlayerInventory>();
            if (inv != null && !inv.HasKey)
            {
                inv.PickUpKey();
                PlaySound();
                Destroy(gameObject); // delay destroy for sound
            }
        }
        else if(other.CompareTag("PlayerClone"))
        {
            other.GetComponent<CloneTemporaryInventory>()?.CollectKey();
            PlaySound();
            Destroy(gameObject);
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
