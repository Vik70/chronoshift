using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeReversal : MonoBehaviour
{
    public float recordTime = 5f; // Seconds of history to record
    private List<PointInTime> pointsInTime;
    public bool IsRewinding { get; private set; } = false;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public AudioClip rewindSound;
    private AudioSource rewindAudio;

    [Range(0f, 1f)]
    public float rewindVolume = 0.75f;


    void Awake()
    {
        // Cache the Rigidbody2D and SpriteRenderer components
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set up audio source for rewind
        rewindAudio = gameObject.AddComponent<AudioSource>();
        rewindAudio.clip = rewindSound;
        rewindAudio.loop = true;
        rewindAudio.playOnAwake = false;
        rewindAudio.volume = 0.25f; // Adjust as needed
    }

    void Start()
    {
        pointsInTime = new List<PointInTime>();
        // Store the original color at start
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        // Input handling remains in Update for responsiveness
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRewind();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            StopRewind();
        }
    }

    void FixedUpdate()
    {
        // Do physics-based recording or rewinding in FixedUpdate
        if (IsRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }

    void Record()
    {
        // Record the current position, rotation, and velocity
        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, rb.velocity));

        // Limit history based on recordTime and fixedDeltaTime
        int maxPoints = Mathf.RoundToInt(recordTime / Time.fixedDeltaTime);
        if (pointsInTime.Count > maxPoints)
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }
    }

    void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            // Retrieve the most recent snapshot
            PointInTime point = pointsInTime[0];
            // Restore transform and velocity
            transform.position = point.position;
            transform.rotation = point.rotation;
            rb.velocity = point.velocity;
            // Remove the snapshot so we move backwards in time
            pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }
    }

    public void StartRewind()
    {
        IsRewinding = true;
        rb.isKinematic = true;
        spriteRenderer.color = Color.gray;

        if (rewindAudio != null && rewindSound != null)
        {
            rewindAudio.Play();
        }
        Debug.Log("Rewind started");
    }

    IEnumerator FadeOutRewind(float duration = 0.5f)
    {
        float startVol = rewindAudio.volume;
        float t = 0f;

        while (t < duration)
        {
            rewindAudio.volume = Mathf.Lerp(startVol, 0f, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        rewindAudio.Stop();
        rewindAudio.volume = startVol;
    }


    public void StopRewind()
    {
        IsRewinding = false;
        // Re-enable physics simulation
        rb.isKinematic = false;
        // Revert the player's color back to the original color
        spriteRenderer.color = originalColor;

        if (rewindAudio != null && rewindAudio.isPlaying)
        {
            StartCoroutine(FadeOutRewind());
        }
    }

    void OnDrawGizmos()
    {
        // Visualize recorded points in the Scene view for debugging
        if (pointsInTime != null)
        {
            Gizmos.color = Color.cyan;
            foreach (PointInTime point in pointsInTime)
            {
                Gizmos.DrawSphere(point.position, 0.1f);
            }
        }
    }
}



public struct PointInTime
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;  // Record velocity to restore physics accurately

    public PointInTime(Vector3 pos, Quaternion rot, Vector3 vel)
    {
        position = pos;
        rotation = rot;
        velocity = vel;
    }
}
