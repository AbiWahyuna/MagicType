using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 6f;
    public float lifeTime = 3f;

    private Transform target;
    private Vector3 direction;

    void Start()
    {
        Destroy(gameObject, lifeTime);

        // cari enemy TERDEKAT SAAT LAHIR
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float nearest = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject e in enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < nearest)
            {
                nearest = dist;
                closest = e.transform;
            }
        }

        if (closest != null)
        {
            target = closest;
            direction = (target.position - transform.position).normalized;
        }
        else
        {
            // fallback kalau belum ada musuh
            direction = transform.right;
        }
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            EnemyHealth eh = col.GetComponentInParent<EnemyHealth>();
            if (eh != null)
                eh.TakeDamage(5f);

            Destroy(gameObject);

        }
    }
}
