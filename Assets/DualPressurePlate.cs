using UnityEngine;

public class DualPressurePlate : MonoBehaviour
{
    public bool isPressed { get; private set; } = false;

    public PlateDoor connectedDoor; // Drag your Door in Inspector!

    [Header("Audio")]
    public AudioClip platePressSound;
    public AudioClip plateReleaseSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // Make sure the plate has an AudioSource!
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerClone"))
        {
            isPressed = true;
            if (audioSource && platePressSound)
                audioSource.PlayOneShot(platePressSound);

            connectedDoor.CheckPlates();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerClone"))
        {
            isPressed = false;
            if (audioSource && plateReleaseSound)
                audioSource.PlayOneShot(plateReleaseSound);

            connectedDoor.CheckPlates();
        }
    }
}
