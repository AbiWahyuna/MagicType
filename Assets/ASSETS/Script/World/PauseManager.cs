using System.Collections;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [Header("Pause UI")]
    public GameObject pausePanel;
    public CanvasGroup pauseCanvas;
    public float fadeDuration = 0.3f;

    private bool isPaused = false;
    private bool canPause = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        pausePanel.SetActive(false);
        pauseCanvas.alpha = 0f;
    }

    void Update()
    {
        if (!canPause) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    // ===== PUBLIC =====
    public void Pause()
    {
        if (isPaused) return;

        isPaused = true;
        pausePanel.SetActive(true);
        StartCoroutine(Fade(0f, 1f));
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (!isPaused) return;

        StartCoroutine(ResumeRoutine());
    }

    public void DisablePause()
    {
        canPause = false;
    }

    // ===== COROUTINE =====
    IEnumerator ResumeRoutine()
    {
        Time.timeScale = 1f;
        yield return StartCoroutine(Fade(1f, 0f));
        pausePanel.SetActive(false);
        isPaused = false;
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        pauseCanvas.alpha = from;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime; // 🔥 tetap jalan walau pause
            pauseCanvas.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }

        pauseCanvas.alpha = to;
    }
}
