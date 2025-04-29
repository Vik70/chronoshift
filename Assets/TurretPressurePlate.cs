using UnityEngine;

public class TurretPressurePlate : MonoBehaviour
{
    public LaserTurret connectedTurret;
    public AudioClip platePressSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerClone"))
        {
            if (connectedTurret != null)
                connectedTurret.SetFiringState(true);

            if (audioSource && platePressSound)
                audioSource.PlayOneShot(platePressSound);
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player") || collision.CompareTag("PlayerClone"))
    //    {
    //        if (connectedTurret != null)
    //            connectedTurret.SetFiringState(false);
    //    }
    //}
}
