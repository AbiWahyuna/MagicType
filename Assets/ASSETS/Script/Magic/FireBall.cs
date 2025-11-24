using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 6f;
    public float lifeTime = 3f;

    private Vector3 direction;

    // Dipanggil sekali dari SpellManager waktu spawn
    public void Init(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // TODO: Add damage system later
            Destroy(gameObject);
        }
    }
}
