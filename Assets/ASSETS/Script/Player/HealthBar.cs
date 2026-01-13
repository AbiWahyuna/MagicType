using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float health;

    public Slider easeHealthBar;
    private float lerpSpeed = 2f;



    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
        easeHealthBar.maxValue = maxHealth;
        easeHealthBar.value = health;
    }


    // Update is called once per frame
    void Update()
    {
        if(healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            takedamage(10f);
        }

       if(Mathf.Abs(easeHealthBar.value - healthSlider.value) > 0.1)
       {
            easeHealthBar.value = Mathf.Lerp(
                easeHealthBar.value,
                healthSlider.value,
                Time.deltaTime * lerpSpeed
                );
        }
        else
        {
            easeHealthBar.value = healthSlider.value;
        }
    }

    void takedamage(float damage)
    {
        health -= damage;
    }
}
