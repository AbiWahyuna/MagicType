using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthBar;

    public float maxHealth = 100f;
    public float health;

    private float lerpSpeed = 10f;

    [Header("LosePanel")]
    public System.Action OnDeath;



    void Start()
    {
        health = maxHealth;

        healthSlider.maxValue = maxHealth;
        easeHealthBar.maxValue = maxHealth;

        healthSlider.value = health;
        easeHealthBar.value = health;

       
    }

    void Update()
    {
        // Hard bar langsung
        healthSlider.value = health;

        // Smooth bar
        if (Mathf.Abs(easeHealthBar.value - health) > 0.01f)
        {
            easeHealthBar.value = Mathf.Lerp(
                easeHealthBar.value,
                health,
                Time.deltaTime * lerpSpeed
            );
        }
        else
        {
            easeHealthBar.value = health;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health <= 0)
        {
            health = 0;
            OnDeath?.Invoke();
        }

    }

    void Die()
    {
      
    }
}
