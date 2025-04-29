using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ComicCutscene : MonoBehaviour
{
    public Sprite[] comicPanels;
    public Image comicImage;  // The UI Image that shows the panels
    public float panelDisplayTime = 2f;
    public float fadeTime = 0.5f;
    public string nextSceneName = "Level1";

    [Header("Audio")]
    public AudioClip backgroundMusic;
    private AudioSource musicSource;

    private int currentIndex = 0;

    void Start()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = 0f; // start silent
        musicSource.Play();

        StartCoroutine(FadeMusicIn(1.5f)); // fade in over 1.5 seconds
        StartCoroutine(PlayCutscene());

    }

    IEnumerator FadeMusicIn(float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            musicSource.volume = Mathf.Lerp(0f, 0.1f, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        musicSource.volume = 0.1f;
    }

    IEnumerator FadeMusicOut(float duration)
    {
        float startVolume = musicSource.volume;
        float t = 0f;
        while (t < duration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        musicSource.volume = 0f;
        musicSource.Stop();
    }

    IEnumerator PlayCutscene()
    {
        for (int i = 0; i < comicPanels.Length; i++)
        {
            comicImage.sprite = comicPanels[i];
            yield return StartCoroutine(FadeIn());
            yield return new WaitForSeconds(panelDisplayTime);
            yield return StartCoroutine(FadeOut());
        }

        // After final panel, load Level 1

        yield return StartCoroutine(FadeMusicOut(1.5f)); // fade out before scene switch
        SceneManager.LoadScene(nextSceneName);

    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        Color c = comicImage.color;
        while (timer < fadeTime)
        {
            c.a = Mathf.Lerp(0f, 1f, timer / fadeTime);
            comicImage.color = c;
            timer += Time.deltaTime;
            yield return null;
        }
        c.a = 1f;
        comicImage.color = c;
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;
        Color c = comicImage.color;
        while (timer < fadeTime)
        {
            c.a = Mathf.Lerp(1f, 0f, timer / fadeTime);
            comicImage.color = c;
            timer += Time.deltaTime;
            yield return null;
        }
        c.a = 0f;
        comicImage.color = c;
    }
}
