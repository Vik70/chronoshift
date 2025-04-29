using UnityEngine;

public class PlateDoor : MonoBehaviour
{
    public DualPressurePlate plate1;
    public DualPressurePlate plate2;

    public AudioClip doorOpen;

    private Animator animator;
    private Collider2D doorCollider;
    private AudioSource audioSource;
    private bool doorOpened = false; // NEW


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        doorCollider = GetComponent<Collider2D>();
    }

    public void CheckPlates()
    {
        if (plate1.isPressed && plate2.isPressed)
        {
            if (!doorOpened) // Only open once!
            {
                if (audioSource && doorOpen) audioSource.PlayOneShot(doorOpen);

                OpenDoor();
                doorOpened = true;
            }
        }
    }

    private void OpenDoor()
    {
        if (animator) animator.SetTrigger("open");
        if (doorCollider) doorCollider.enabled = false;
        Debug.Log("Door opened!");
    }

    private void CloseDoor()
    {
        
        if (doorCollider) doorCollider.enabled = true;
        Debug.Log("Door closed!");
    }
}
