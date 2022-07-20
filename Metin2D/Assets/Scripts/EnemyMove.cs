using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float circleRange;
    [SerializeField] Transform wallDetector;
    Enemy enemy;
    public int direction;
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
        rb.velocity = new Vector2(direction * enemy.enemiesSpeed, rb.velocity.y);
        transform.localScale=new Vector3(-direction,1,1);
        Collider2D hitGround = Physics2D.OverlapCircle(wallDetector.position, circleRange, LayerMask.GetMask("Ground"));
        if (hitGround != null && Time.time >= time)
        {
            direction *= -1;
            time = Time.time + coolDown;
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(wallDetector.position, circleRange);
    }
}
