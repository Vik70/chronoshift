using UnityEngine;

public class VerticalMovingPlatform : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float moveDistance = 7.0f;
    private Vector3 startPos;
    private float elapsedTime = 0f;

    // Instead of disabling the script, we control the movement with this flag.
    public bool isPaused = false;

    void Start()
    {
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        // Only increase elapsedTime if the platform isn't paused.
        if (!isPaused)
        {
            elapsedTime += Time.fixedDeltaTime;
        }

        // Use our own elapsedTime to drive the PingPong movement.
        float yOffset = Mathf.PingPong(elapsedTime * moveSpeed, moveDistance);
        transform.position = startPos - new Vector3(0f, yOffset, 0f);
    }

    // Call this to pause the platform movement.
    public void PausePlatform()
    {
        isPaused = true;
    }

    // Call this to resume the platform movement.
    public void ResumePlatform()
    {
        isPaused = false;
    }

    // Optionally, you can leave collision parent methods as-is.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}
