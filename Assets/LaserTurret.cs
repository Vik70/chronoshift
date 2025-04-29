using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform firePoint;
    public Vector2 fireDirection = Vector2.left;
    public float fireInterval = 2f;
    public float laserSpeed = 10f;

    private bool isFrozen = false;
    private bool isFiring = false;  // <- Firing on pressure plate
    private float fireTimer = 0f;

    [Header("Audio")]
    public AudioClip fireSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isFrozen || !isFiring) return;

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            FireLaser();
            fireTimer = 0f;
        }
    }

    void FireLaser()
    {
        GameObject laser = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
        LaserBeam beam = laser.GetComponent<LaserBeam>();

        if (beam != null)
        {
            beam.direction = fireDirection;
            beam.speed = laserSpeed;
        }

        if (audioSource && fireSound)
            audioSource.PlayOneShot(fireSound);
    }

    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;
    }

    public void SetFiringState(bool state)
    {
        isFiring = state;
        if (!state) fireTimer = 0f;
    }
}
