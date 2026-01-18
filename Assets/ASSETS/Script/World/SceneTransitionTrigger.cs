using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionTrigger : MonoBehaviour
{
    [Header("Scene")]
    public string targetScene;

    [Header("Fade")]
    public Image fadePanel;
    public float fadeDuration = 1f;

    private bool isTransitioning = false;
    private Rigidbody2D playerRb;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            isTransitioning = true;

            // FREEZE PLAYER
            playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = Vector2.zero;
                playerRb.simulated = false; // MATI TOTAL
            }

            StartCoroutine(Transition());
        }
    }

    IEnumerator Transition()
    {
        yield return StartCoroutine(Fade(0f, 1f));
        SceneManager.LoadScene(targetScene);
        yield return null;
        yield return StartCoroutine(Fade(1f, 0f));
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timer = 0f;
        Color color = fadePanel.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            fadePanel.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadePanel.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
