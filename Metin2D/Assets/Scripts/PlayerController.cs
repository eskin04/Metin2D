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
    [SerializeField] GameObject dashWind;
    [SerializeField] GameObject fireBallPref;
    Rigidbody2D platformRb;
    BoxCollider2D boxCollider;
    int health = 7;
    bool isGrounded = true;
    float nextAttackTime;
    bool isKnockBack;
    bool isDash;
    bool dashCoolDown;
    int isLookRight;
    float firstGravityScale;
    bool isInLadder;
    bool isOnPlatform;
    bool isGameOver;

    
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
        if(!isGameOver)
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

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isDash && !isKnockBack)
            {
                Jump();
            }
            if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
            {
                anim.SetTrigger("Attack");
            }
            if (health <= 0)
            {
                GameOver();
            }
            if (Input.GetMouseButtonDown(1) && !dashCoolDown && !isKnockBack)
            {
                dashCoolDown = true;
                Dash();
            }
            if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextAttackTime)
            {
                anim.SetTrigger("FireBall");
            }
        }

    }
    void Dash()
    {
        isDash = true;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.right * isLookRight * dashForce, ForceMode2D.Impulse);
        anim.SetBool("isDash", true);
        dashWind.SetActive(true);
        StartCoroutine(DashWait());
    }
    IEnumerator DashWait()
    {
        yield return new WaitForSeconds(.2f);
        rb.velocity = Vector2.zero;
        rb.gravityScale = firstGravityScale;
        isDash = false;
        anim.SetBool("isDash", false);
        dashWind.SetActive(false);
        yield return new WaitForSeconds(.4f);
        dashCoolDown = false;
    }
    void GameOver()
    {
        rb.velocity = Vector3.zero;
        isGameOver = true;
        anim.SetTrigger("Die");
        StartCoroutine(WaitGameOver());
    }
    IEnumerator WaitGameOver()
    {
        yield return new WaitForSeconds(.7f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(.5f);
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
    void FireBall()
    {
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y - .5f);
        nextAttackTime = Time.time + 1f / attackRate;
        fireBallPref.transform.localScale = transform.localScale;
        Instantiate(fireBallPref, playerPos, Quaternion.identity);

    }
    void Attack()
    {
        nextAttackTime = Time.time + 1f / attackRate;

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
        Collider2D[] secretPlace = Physics2D.OverlapCircleAll(attackPos.position,attackRange, LayerMask.GetMask("SecretPlace"));
        foreach(Collider2D place in secretPlace)
        {
            if (place != null)
            {
            place.GetComponent<SecretPlace>().DestroyPlace(-1);
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
        public void KnockBackSpike(Vector3 enemyPos)
    {
        isKnockBack = true;
        anim.SetTrigger("Hurt");
        Vector2 knockBackDir = transform.position - enemyPos;
        knockBackDir.Normalize();
        rb.AddForce(knockBackDir * 10, ForceMode2D.Impulse);
        StartCoroutine(KnockBackTimerSpike());

    }


    
    IEnumerator KnockBackTimer(GameObject enemy)
    {
        yield return new WaitForSeconds(.1f);
        anim.SetBool("isHurt", true);
        yield return new WaitForSeconds(0.5f);
        isKnockBack = false;
        anim.SetBool("isHurt", false);
        yield return new WaitForSeconds(.5f);
        
        if(enemy!=null)
        {
            Physics2D.IgnoreCollision(boxCollider, enemy.GetComponent<Collider2D>(), false);
        }

    }
    IEnumerator KnockBackTimerSpike()
    {
        yield return new WaitForSeconds(.1f);
        anim.SetBool("isHurt", true);
        yield return new WaitForSeconds(0.5f);
        isKnockBack = false;
        anim.SetBool("isHurt", false);


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
        if (collision.gameObject.tag == "Spike")
        {
            UpdateHealth(-1);
            KnockBackSpike(collision.transform.position);
            
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
        if (other.gameObject.tag == "Heal")
        {
            UpdateHealth(1);
            Destroy(other.gameObject);
        }
        if(other.gameObject.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (other.gameObject.tag == "Ladder")
        {
            isInLadder = true;
            ladderGround.GetComponent<Collider2D>().enabled = false;

        }
        if (other.gameObject.tag == "DeathArea")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
