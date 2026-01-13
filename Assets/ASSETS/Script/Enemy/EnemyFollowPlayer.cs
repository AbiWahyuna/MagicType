using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float speed;
    public float lineOfSet;
    public Transform player;

    //bullet
    public float shootingRange;
    public GameObject bullet;
    public GameObject bulletParent;
    private Transform target;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distancefromplayer = Vector2.Distance(transform.position, player.position);
        if(distancefromplayer < lineOfSet)
            transform.position = Vector2.MoveTowards(this.transform.position,player.position, speed*Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSet);
    }
}
