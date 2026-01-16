using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float speed;
    public float lineOfSet;
    public Transform player;

    // Bullet
    public float fireRate;
    public float nextFiretime;
    public float shootingRange;
    public GameObject bullet;
    public GameObject bulletParent;

    private Animator animator;
    private Vector2 movementDir;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        Vector2 dir = (player.position - transform.position).normalized;

        // Update arah TERUS (penting)
        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);

        if (distance < lineOfSet && distance > shootingRange)
        {
            // GERAK + RUN NORMAL
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );

            animator.SetBool("IsMoving", true);
        }
        else if (distance <= shootingRange)
        {
            // DIAM TAPI ANIMASI TETAP RUN
            animator.SetBool("IsMoving", true);

            if (nextFiretime < Time.time)
            {
                Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
                nextFiretime = Time.time + fireRate;
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSet);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
