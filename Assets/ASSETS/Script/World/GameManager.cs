using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public float spawnDelay = 0.7f;

    [Header("Wave Settings")]
    public int totalWave = 5;
    public int baseEnemyCount = 5;
    public float waveDelay = 2f;

    [Header("Wave UI")]
    public GameObject wavePanel;
    public CanvasGroup waveCanvas;
    public TMP_Text waveText;
    public float fadeDuration = 0.5f;
    public float showDuration = 1.5f;

    private int currentWave = 0;
    private int enemyAlive = 0;
    private bool isSpawning = false;

    [Header("Win UI")]
    public GameObject winPanel;

    [Header("Lose UI")]
    public GameObject losePanel;
    private bool gameEnded = false;





    void Start()
    {
        HealthBar hb = FindObjectOfType<HealthBar>();
        if (hb != null)
            hb.OnDeath += LoseGame;

        wavePanel.SetActive(false);
        StartCoroutine(WaveRoutine());

        winPanel.SetActive(false);
        losePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    IEnumerator WaveRoutine()
    {
        while (currentWave < totalWave)
        {
            currentWave++;

            yield return StartCoroutine(ShowWaveBanner());

            int enemyToSpawn = baseEnemyCount + (currentWave * 2);
            yield return StartCoroutine(SpawnWave(enemyToSpawn));

            // tunggu semua musuh mati
            yield return new WaitUntil(() => enemyAlive <= 0);
            yield return new WaitForSeconds(waveDelay);
        }

        Debug.Log("SEMUA WAVE SELESAI 🎉");
        WinGame();
    }

    void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        StartCoroutine(WinDelay());
    }


    IEnumerator WinDelay()
    {
        winPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
    }



    public void LoseGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        StartCoroutine(LoseDelay());
    }


    IEnumerator LoseDelay()
    {
        losePanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
    }




    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }


    IEnumerator SpawnWave(int amount)
    {
        isSpawning = true;

        for (int i = 0; i < amount; i++)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawn.position, Quaternion.identity);
            enemyAlive++;
            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawning = false;
    }

    IEnumerator ShowWaveBanner()
    {
        wavePanel.SetActive(true);
        waveText.text = "WAVE " + currentWave;

        // fade in
        yield return StartCoroutine(FadeCanvas(0f, 1f));
        yield return new WaitForSeconds(showDuration);
        // fade out
        yield return StartCoroutine(FadeCanvas(1f, 0f));

        wavePanel.SetActive(false);
    }

    IEnumerator FadeCanvas(float from, float to)
    {
        float t = 0f;
        waveCanvas.alpha = from;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            waveCanvas.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }

        waveCanvas.alpha = to;
    }

    // dipanggil musuh saat mati
    public void EnemyKilled()
    {
        enemyAlive--;
    }
}
