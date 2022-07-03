using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float circleRange;
    [SerializeField] Transform wallDetector;
    Enemy enemy;
    int direction;
    float time;
    float coolDown;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        direction = 1;
        coolDown = .5f;
    }

    // Update is called once per frame
    void Update()
    {


        // Physics2D.queriesStartInColliders = false;
        rb.velocity = new Vector2(direction * enemy.enemiesSpeed, rb.velocity.y);
        // RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * rightDirection, maxDistance + 2, LayerMask.GetMask("Ground"));
        // Debug.DrawRay(transform.position, Vector2.right * maxDistance * rightDirection, Color.red);
        // if (hit.collider != null)
        // {
        //     if (hit.distance <= 1)
        //     {
        //         direction = -1;
        //     }
        //     else if (hit.distance >= maxDistance)
        //     {
        //         direction = 1;
        //     }
        // }

        Collider2D hitGround = Physics2D.OverlapCircle(wallDetector.position, circleRange, LayerMask.GetMask("Ground"));
        if (hitGround != null && Time.time >= time)
        {
            direction *= -1;
            transform.localScale=new Vector3(transform.localScale.x*-1,1,1);
            time = Time.time + coolDown;
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(wallDetector.position, circleRange);
    }
}
