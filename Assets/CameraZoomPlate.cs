using UnityEngine;

public class CameraZoomPlate : MonoBehaviour
{
    public Animator cameraAnimator;               
    public MovingPlatform[] platformsToActivate;  
    public AudioClip plateSound;                  

    private AudioSource audioSource;
    private bool triggered = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player") || collision.CompareTag("PlayerClone"))
        {
            triggered = true;

         
            if (cameraAnimator != null)
            {
                cameraAnimator.SetTrigger("ZoomOut");
            }

           
            foreach (MovingPlatform platform in platformsToActivate)
            {
                platform.ResumePlatform();
            }

            // 🔊 Play sound
            if (audioSource != null && plateSound != null)
            {
                audioSource.PlayOneShot(plateSound, 0.2f);
            }

            Debug.Log("Plate activated: camera zoom and platforms started.");
        }
    }
}
