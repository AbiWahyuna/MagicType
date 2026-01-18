using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float speed = 2f;
    public float lineOfSet = 6f;

    [Header("Attack")]
    public float fireRate = 1f;
    public float shootingRange = 3f;
    public GameObject bullet;
    public Transform bulletParent;

    private float nextFiretime;
    private Transform player;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
            Debug.LogWarning("Player belum ada di scene!");
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        Vector2 dir = (player.position - transform.position).normalized;

        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);

        if (distance <= lineOfSet && distance > shootingRange)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );

            animator.SetBool("IsMoving", true);
        }
        else if (distance <= shootingRange)
        {
            animator.SetBool("IsMoving", true);

            if (Time.time >= nextFiretime)
            {
                Instantiate(bullet, bulletParent.position, Quaternion.identity);
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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
