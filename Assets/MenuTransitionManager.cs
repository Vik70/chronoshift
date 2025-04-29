using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuTransitionManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image blackOverlay;
    public TMP_Text levelText;

    [Header("Audio")]
    public AudioSource menuMusic;
    public AudioClip clickSound;
    public AudioSource sfxSource; // a second AudioSource to play the click

    [Header("Settings")]
    public string levelToLoad = "GameplayScene";
    public float fadeDuration = 1.5f;
    public float waitBeforeLoad = 2.0f;

    public void OnStartButtonPressed()
    {
        // Play button click sound
        if (sfxSource != null && clickSound != null)
        {
            sfxSource.PlayOneShot(clickSound);
        }

        // Start transition
        StartCoroutine(TransitionRoutine());
    }

    IEnumerator TransitionRoutine()
    {
        // Step 1: Fade out music
        if (menuMusic != null)
        {
            float startVolume = menuMusic.volume;
            float t = 0;
            while (t < fadeDuration)
            {
                menuMusic.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
                t += Time.deltaTime;
                yield return null;
            }
            menuMusic.volume = 0f;
        }

        // Step 2: Fade in black overlay
        yield return StartCoroutine(FadeImageAlpha(blackOverlay, 0f, 1f, fadeDuration));

        // Step 3: Fade in "Level 1" text
        yield return StartCoroutine(FadeTextAlpha(levelText, 0f, 1f, 1f));

        // Step 4: Load the next scene
        yield return new WaitForSeconds(waitBeforeLoad);
        SceneManager.LoadScene(levelToLoad);
    }

    IEnumerator FadeImageAlpha(Image img, float from, float to, float duration)
    {
        float timer = 0f;
        Color c = img.color;
        while (timer < duration)
        {
            float t = timer / duration;
            c.a = Mathf.Lerp(from, to, t);
            img.color = c;
            timer += Time.deltaTime;
            yield return null;
        }
        c.a = to;
        img.color = c;
    }

    IEnumerator FadeTextAlpha(TMP_Text txt, float from, float to, float duration)
    {
        float timer = 0f;
        Color c = txt.color;
        while (timer < duration)
        {
            float t = timer / duration;
            c.a = Mathf.Lerp(from, to, t);
            txt.color = c;
            timer += Time.deltaTime;
            yield return null;
        }
        c.a = to;
        txt.color = c;
    }
}
