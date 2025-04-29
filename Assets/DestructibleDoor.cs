using UnityEngine;
using UnityEngine.UI;

public class DestructibleDoor : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    public GameObject healthBarPrefab; // assign in Inspector
    private Slider healthSlider;
    private Canvas gameCanvas;
    private GameObject healthBarInstance;

    private void Start()
    {
        currentHealth = maxHealth;

        // Find your existing UI canvas
        gameCanvas = GameObject.Find("WorldSpaceCanvas").GetComponent<Canvas>();
        healthBarInstance = Instantiate(healthBarPrefab, gameCanvas.transform);

        healthSlider = healthBarInstance.GetComponentInChildren<Slider>();
        healthSlider.value = 1f; // full
    }

    private void Update()
    {
        if (healthBarInstance)
        {
            Vector3 offset = new Vector3(-0.8f, 2.5f, 0f);
            Vector3 worldPos = transform.position + offset;
            healthBarInstance.transform.position = worldPos;
            healthBarInstance.transform.rotation = Quaternion.identity;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthSlider.value = (float)currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        Destroy(gameObject); // or play animation
        Destroy(healthBarInstance); // clean up UI
    }
}
