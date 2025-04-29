using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelTransitionManager : MonoBehaviour
{
    public Image fadeOverlay;
    public TMP_Text resultText;
    public float fadeDuration = 1f;
    public float messageDuration = 2f;
    public string nextSceneName;

    public void StartTransition(bool hasTimeCoin)
    {
        StartCoroutine(PlayLevelTransition(hasTimeCoin));
    }

    IEnumerator PlayLevelTransition(bool success)
    {
        // Optional fade to black
        yield return StartCoroutine(FadeToBlack());

        if (resultText != null)
        {
            resultText.gameObject.SetActive(true);
            resultText.text = success ? "Time Segment Repaired" : "Time was Fractured...";
            Color color = resultText.color;
            color.a = 1f;
            resultText.color = color;
        }

        yield return new WaitForSeconds(messageDuration);

        string sceneToLoad = success ? nextSceneName : SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator FadeToBlack()
    {
        float t = 0f;
        Color c = fadeOverlay.color;
        while (t < fadeDuration)
        {
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeOverlay.color = c;
            t += Time.deltaTime;
            yield return null;
        }
        c.a = 1f;
        fadeOverlay.color = c;
    }
}
