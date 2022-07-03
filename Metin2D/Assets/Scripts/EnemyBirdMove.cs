using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBirdMove : MonoBehaviour
{
    [SerializeField] float attackRange;
    [SerializeField] GameObject playerHead;
    Enemy enemy;
    [SerializeField] Rigidbody2D rb;
    Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Player"));
        if (player != null)
        {
            direction = playerHead.transform.position - transform.position;
            direction.Normalize();
            rb.velocity = direction * enemy.enemiesSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if(direction.x>0)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            transform.localScale = new Vector3(1,1,1);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
