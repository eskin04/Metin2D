using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEnemyMove : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float attackRange;
    [SerializeField] float jumpForce;
    [SerializeField] GameObject playerHead;
    [SerializeField] Transform jumpPoint;
    [SerializeField] float jumpSpeed;
    Animator anim;
    Enemy enemy;
    EnemyMove enemyMove;
    bool isGround = true;
    float time;
    bool isFirstTime;
    [SerializeField] float coolDown;


    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyMove = GetComponent<EnemyMove>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(jumpPoint.position, attackRange, LayerMask.GetMask("Player"));
        if (hitPlayer != null)
        {
            if (isGround && Time.time >=time)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                anim.SetTrigger("isPlayer");
                isGround = false;
                enemy.enemiesSpeed = jumpSpeed;
                isFirstTime = true;
                time =coolDown + Time.time;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(jumpPoint.position, attackRange);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            if(isFirstTime)
            {
                enemyMove.direction*=-1;
            }
            isGround = true;
            enemy.enemiesSpeed = enemy.firstSpeed;
        }
    }
}
