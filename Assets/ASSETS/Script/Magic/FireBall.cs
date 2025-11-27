using UnityEngine;
using UnityEngine.UIElements;

public class Fireball : MonoBehaviour
{
    public float speed = 6f;
    public float lifeTime = 3f;
    public float rotateSpeed = 4f;

    private Transform target;
    private Vector3 direction; //arah awal 

    private void Start()
    {
        Destroy(gameObject, lifeTime);

        //cari musuh terdekat
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float nearest = Mathf.Infinity;

        foreach (GameObject e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist < nearest)
            {
                nearest = dist;
                target = e.transform;
            }
        }
    }
    public void Init(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        if (target == null)
        {
            //tidak ada musuh = gerak lurus
            transform.position += direction * speed * Time.deltaTime;
            return;
        }

        //Arah menuju musuh
        Vector2 toTarget = (target.position - transform.position).normalized;

        //smooth rotate dari lurus lalu belok menuju musuh
        direction = Vector3.Lerp(direction, toTarget, rotateSpeed * Time.deltaTime).normalized;

        //Gerak magic
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            // Damage musuh nanti
            Destroy(gameObject);
        }
    }
}
