using UnityEngine;

public class TutorialTextTrigger : MonoBehaviour
{
    // Rather than providing tutorialText here, ensure the text component on textObject
    // already has the desired text set in the Inspector.
    [Header("Assign the pre-configured text GameObject")]
    public GameObject textObject;

    private void Start()
    {
        // Hide the text object at the start.
        if (textObject != null)
        {
            textObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && textObject != null)
        {
            textObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && textObject != null)
        {
            textObject.SetActive(false);
        }
    }
}
