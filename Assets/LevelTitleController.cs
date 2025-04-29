using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelTitleController : MonoBehaviour
{
    public TMP_Text levelText;
    public Image blackOverlay; // optional
    public string displayText = "Level 1";
    public float fadeDuration = 1f;
    public float visibleDuration = 2f;

    void Start()
    {
        Color textColor = levelText.color;
        textColor.a = 1f;
        levelText.color = textColor;

        if (blackOverlay != null)
        {
            Color bgColor = blackOverlay.color;
            bgColor.a = 1f;
            blackOverlay.color = bgColor;
        }
        levelText.text = displayText;
        StartCoroutine(PlayTitleCard());
    }

    IEnumerator PlayTitleCard()
    {
        //yield return StartCoroutine(Fade(0f, 1f, fadeDuration));  // Fade in
        yield return new WaitForSeconds(visibleDuration);         // Wait
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));  // Fade out
        Destroy(gameObject);                                      // Clean up
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0f;
        Color textColor = levelText.color;
        Color bgColor = blackOverlay != null ? blackOverlay.color : Color.clear;

        while (t < duration)
        {
            float alpha = Mathf.Lerp(from, to, t / duration);
            levelText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            if (blackOverlay != null)
                blackOverlay.color = new Color(bgColor.r, bgColor.g, bgColor.b, alpha);

            t += Time.deltaTime;
            yield return null;
        }

        levelText.color = new Color(textColor.r, textColor.g, textColor.b, to);
        if (blackOverlay != null)
            blackOverlay.color = new Color(bgColor.r, bgColor.g, bgColor.b, to);
    }
}
