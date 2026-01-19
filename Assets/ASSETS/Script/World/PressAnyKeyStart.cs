using System.Collections;
using UnityEngine;

public class PressAnyKeyStart : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    private bool started = false;

    void Start()
    {
        Time.timeScale = 0f;

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    void Update()
    {
        if (started) return;

        if (Input.anyKeyDown)
        {
            started = true;
            StartCoroutine(FadeOutAndStart());
        }
    }

    IEnumerator FadeOutAndStart()
    {
        float t = 0f;

        // balikin waktu biar fade jalan
        Time.timeScale = 1f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        gameObject.SetActive(false);
    }
}
