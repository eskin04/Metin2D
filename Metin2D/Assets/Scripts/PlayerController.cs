using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform attackPos;
    [SerializeField] float attackRange;
    [SerializeField] float speed;
    [SerializeField] float dashForce;
    [SerializeField] float jumpForce;
    [SerializeField] float attackRate;
    [SerializeField] float upSpeed;
    [SerializeField] Animator anim;
    [SerializeField] Vector3 localScale;
    [SerializeField] Vector3 reverseScale;
    [SerializeField] HealthBar healthBar;
    [SerializeField] GameObject ladderGround;

    Rigidbody2D platformRb;
    BoxCollider2D boxCollider;
    int health = 7;
    bool isGrounded = true;
    float nextAttackTime;
    bool isKnockBack;
    bool isDash;
    bool dashCoolDown;
    int isLookRight = 1;
    float firstGravityScale;
    bool isInLadder;
    bool isOnPlatform;


    // Start is called before the first frame update
    void Start()
    {
        firstGravityScale = rb.gravityScale;
        boxCollider = GetComponent<BoxCollider2D>();
        healthBar.SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("AirSpeed", rb.velocity.y);
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (isInLadder)
        {
            UpAndDown(verticalInput);
        }
        if (!isKnockBack && !isDash)
        {
            Move(horizontalInput);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isDash)
        {
            Jump();
        }
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            Attack();
        }
        if (health <= 0)
        {
            GameOver();
        }
        if (Input.GetMouseButtonDown(1) && !dashCoolDown)
        {
            dashCoolDown = true;
            Dash();
        }

    }
    void Dash()
    {
        isDash = true;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.right * isLookRight * dashForce, ForceMode2D.Impulse);
        StartCoroutine(DashWait());
    }
    IEnumerator DashWait()
    {
        yield return new WaitForSeconds(.2f);
        rb.velocity = Vector2.zero;
        rb.gravityScale = firstGravityScale;
        isDash = false;
        yield return new WaitForSeconds(.4f);
        dashCoolDown = false;
    }
    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpAndDown(float vertical){

        if(vertical>=.2f){
            rb.velocity = new Vector2(rb.velocity.x, upSpeed);
            rb.gravityScale = firstGravityScale;
            Debug.Log("up");
        }
        else if (vertical<=-.2f) {
            rb.velocity = new Vector2(rb.velocity.x, -upSpeed);
            rb.gravityScale = firstGravityScale;
        }
        else{
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.gravityScale = 0;
            Debug.Log("stop");

        }
    }
    void Attack()
    {
        nextAttackTime = Time.time + 1f / attackRate;
        anim.SetTrigger("Attack");

        // find enemies in range
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(1);
        }

        // find boss
        Collider2D boss = Physics2D.OverlapCircle(attackPos.position, attackRange, LayerMask.GetMask("Boss"));
        if (boss != null)
        {
            boss.GetComponent<BossController>().TakeDamage(1);
        }

        // find chests in range
        Collider2D[] chests = Physics2D.OverlapCircleAll(attackPos.position, attackRange, LayerMask.GetMask("Chest"));
        foreach (Collider2D chest in chests)
        {
            // if chest is close, open it
            if(chest.GetComponent<Chest>().isOpen == false)
            {
                chest.GetComponent<Chest>().OpenChest();
                Debug.Log("Chest opened");
            }
            // if chest is open, close it
            else
            {
                chest.GetComponent<Chest>().CloseChest();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
    void Move(float horizontalInput)
    {
        if(isOnPlatform)
        {
            if(horizontalInput==0)
            {
                rb.velocity = new Vector2(platformRb.velocity.x,rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y) + platformRb.velocity;
            }
        }
        else 
        {
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }

        if (horizontalInput != 0)
        {
            anim.SetInteger("AnimState", 2);
        }
        else
        {
            anim.SetInteger("AnimState", 0);
        }
        if (horizontalInput < 0)
        {
            transform.localScale = localScale;
            isLookRight = -1;
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = reverseScale;
            isLookRight = 1;
        }
    }
    void Jump()
    {
        isGrounded = false;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        anim.SetTrigger("Jump");
    }

    public void UpdateHealth(int add)
    {
        health += add;
        healthBar.SetHealth(health);
    }
    public void KnockBack(Vector3 enemyPos, GameObject enemy)
    {
        isKnockBack = true;
        anim.SetTrigger("Hurt");
        Vector2 knockBackDir = transform.position - enemyPos;
        knockBackDir.Normalize();
        rb.AddForce(knockBackDir * 3, ForceMode2D.Impulse);
        Physics2D.IgnoreCollision(boxCollider, enemy.GetComponent<Collider2D>());
        StartCoroutine(KnockBackTimer(enemy));

    }
    IEnumerator KnockBackTimer(GameObject enemy)
    {
        yield return new WaitForSeconds(0.5f);
        isKnockBack = false;
        yield return new WaitForSeconds(.5f);
        if(enemy!=null)
        {
            Physics2D.IgnoreCollision(boxCollider, enemy.GetComponent<Collider2D>(), false);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            anim.SetBool("Grounded", true);
        }
        if (collision.gameObject.tag == "Platform")
        {
            isOnPlatform = true;
            isGrounded = true;
            anim.SetBool("Grounded", true);
            platformRb=collision.gameObject.GetComponent<Rigidbody2D>();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            UpdateHealth(-1);
            KnockBack(collision.transform.position, collision.gameObject);
        }
        if (collision.gameObject.tag == "Laser")
        {
            UpdateHealth(-2);
            KnockBack(collision.transform.position, collision.gameObject);
        }
        if (collision.gameObject.tag == "Bullet")
        {
            UpdateHealth(-1);
            KnockBack(collision.transform.position, collision.gameObject);
        }
        if (collision.gameObject.tag == "Boss")
        {
            UpdateHealth(-2);
            KnockBack(collision.transform.position, collision.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            anim.SetBool("Grounded", false);
        }
        if (other.gameObject.tag == "Platform")
        {
            isOnPlatform = false;
            anim.SetBool("Grounded", false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
        }
        if(other.gameObject.name == "Boss Scene Trigger"){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            
        }
        if (other.gameObject.tag == "Ladder")
        {
            isInLadder = true;
            ladderGround.GetComponent<Collider2D>().enabled = false;

        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            isInLadder = false;
            rb.gravityScale = firstGravityScale;
            anim.SetBool("Grounded", true);
            isGrounded = true;
            ladderGround.GetComponent<Collider2D>().enabled = true;

        }
    }




}
