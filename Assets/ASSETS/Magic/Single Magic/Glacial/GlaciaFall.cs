using UnityEngine;

public class Glaciafall : MonoBehaviour
{
    [Header("Targeting")]
    public float searchRadius = 8f;

    [Header("Damage")]
    public int damage = 30;
    public float lifeTime = 1.2f;

    void Start()
    {
        Transform target = FindNearestEnemy();

        if (target == null)
        {
            Debug.Log("❄️ Glaciafall: no enemy found");
            Destroy(gameObject);
            return;
        }

        // TELEPORT KE MUSUH
        transform.position = target.position;

        // DAMAGE LANGSUNG
        EnemyHealth hp = target.GetComponent<EnemyHealth>();
        if (hp != null)
        {
            hp.TakeDamage(damage);
        }

        // AUTO DESTROY
        Destroy(gameObject, lifeTime);
    }

    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);

            if (dist < minDist && dist <= searchRadius)
            {
                minDist = dist;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }
}
