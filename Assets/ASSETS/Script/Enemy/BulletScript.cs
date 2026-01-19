using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    GameObject target;
    public float speed;
    Rigidbody2D bulletRB;
    public float damage = 10f;


    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDirection = (target.transform.position - transform.position).normalized * speed;
        bulletRB.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Destroy(this.gameObject, 2);
    }




    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthBar health = collision.GetComponentInChildren<HealthBar>();

            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log("PLAYER KENA DAMAGE");
            }
            else
            {
                
            }

            Destroy(gameObject);
        }
    }

}
