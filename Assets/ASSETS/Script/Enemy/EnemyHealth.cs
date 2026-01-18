using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 5f;
    public float currentHealth;

    public float fadeDuration = 0.8f;

    private SpriteRenderer[] sprites;
    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // Matikan semua logic enemy
        GetComponent<EnemyFollowPlayer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);

            foreach (SpriteRenderer sr in sprites)
            {
                if (sr != null)
                {
                    Color c = sr.color;
                    c.a = alpha;
                    sr.color = c;
                }
            }

            yield return null;
        }

        Destroy(gameObject);
        FindObjectOfType<GameManager>().EnemyKilled();
    }
}
