using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour
{
    public GameObject deathCanvas; // Assign the death Canvas (black screen + "phasing back" text)
    public float fadeDuration = 1.5f; // How quickly to fade to black
    public float holdDuration = 2f;   // How long to stay on black screen before restart

    private CanvasGroup deathCanvasGroup;

    private void Awake()
    {
        if (deathCanvas != null)
        {
            deathCanvasGroup = deathCanvas.GetComponent<CanvasGroup>();

            if (deathCanvasGroup == null)
            {
                deathCanvasGroup = deathCanvas.AddComponent<CanvasGroup>();
            }

            deathCanvas.SetActive(false); // Start disabled
        }
    }

    public void HandleDeath()
    {
        StartCoroutine(FadeAndRestart());
    }

    private IEnumerator FadeAndRestart()
    {
        deathCanvas.SetActive(true);
        deathCanvasGroup.alpha = 0f;

        float timer = 0f;

        // Fade in
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            deathCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            yield return null;
        }

        deathCanvasGroup.alpha = 1f;

        // Hold the black screen with "Phasing back..." text
        yield return new WaitForSeconds(holdDuration);

        // Restart scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
