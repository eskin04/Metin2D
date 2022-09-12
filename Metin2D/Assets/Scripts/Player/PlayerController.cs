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
    [SerializeField] float slowMotion;
    [SerializeField] float downAttackForce;
    [SerializeField] public float fireBallCount;
    [SerializeField] Animator anim;
    [SerializeField] Vector3 localScale;
    [SerializeField] Vector3 reverseScale;
    [SerializeField] Collider2D ladderGround;
    [SerializeField] GameObject dashWind;
    [SerializeField] GameObject fireBallPref;
    [SerializeField] GameObject protection;
    [SerializeField] public CanvasManager canvasManager;
    PlayerSound playerSound;
    PlayerData playerDataSc;
    PlayerHealth playerHealthSc;
    Rigidbody2D platformRb;
    BoxCollider2D boxCollider;
    Vector3 exitGroundPos;
    bool isGrounded = true;
    float nextAttackTime;
    public bool isKnockBack;
    public bool isBossDie;
    public bool isBossIntro;
    public bool isInLadder;

    bool isDash;
    bool dashCoolDown;
    int isLookRight;
    float firstGravityScale;
    float fixedTime;
    bool isOnPlatform;
    bool isGameOver;
    bool isProtection;
    bool isIgnoreLayer;
    float waitLayerTime;
    bool isDownAttack;
    bool isParshment;




    // Start is called before the first frame update
    void Start()
    {
        playerDataSc = GetComponent<PlayerData>();
        playerHealthSc = GetComponent<PlayerHealth>();
        playerSound = GetComponent<PlayerSound>();
        IgnoreLayerFalse();
        firstGravityScale = rb.gravityScale;
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        fixedTime = Time.fixedDeltaTime;
    }
    public void CoinUpdate(float coin)
    {
        playerDataSc.totalCoin += coin;
        if (coin > 0)
        {
            playerSound.CoinSound();
        }
        canvasManager.CoinText(playerDataSc.totalCoin);
    }
    public void SaveDashUpgrade()
    {
        playerSound.ShopSound();
        CoinUpdate(-playerDataSc.updateCoinDash);
        playerDataSc.updateCoinDash += 5;
        playerDataSc.dashCoolDown -= 0.2f;
        playerDataSc.Save();
    }
    public void SaveFireUpgrade()
    {
        playerSound.ShopSound();
        CoinUpdate(-playerDataSc.updateCoinFire);
        playerDataSc.updateCoinFire += 5;
        playerDataSc.maxFireBall += 2;
        playerDataSc.Save();
    }
    void IgnoreLayerFalse()
    {
        Physics2D.IgnoreLayerCollision(7, 9, false);
        Physics2D.IgnoreLayerCollision(7, 6, false);
        Physics2D.IgnoreLayerCollision(7, 11, false);
        Physics2D.IgnoreLayerCollision(7, 16, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver && !isBossIntro && !canvasManager.isNextLevelPanel)
        {
            anim.SetFloat("AirSpeed", rb.velocity.y);
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // After KnockBack set LayerCollision to false
            if (!isGameOver && !isBossDie && !isProtection && (Physics2D.GetIgnoreLayerCollision(7, 9) || Physics2D.GetIgnoreLayerCollision(7, 6) || Physics2D.GetIgnoreLayerCollision(7, 11) || Physics2D.GetIgnoreLayerCollision(7, 16)))
            {

                if (!isIgnoreLayer)
                {
                    waitLayerTime = Time.time;
                    isIgnoreLayer = true;
                }
                if (Time.time >= waitLayerTime + 1.12f && isIgnoreLayer)
                {
                    IgnoreLayerFalse();
                    isIgnoreLayer = false;


                }
            }

            //When Player climb or move down at a ladder
            if (isInLadder)
            {
                UpAndDown(verticalInput);
            }

            // Player right and left move
            if (!isKnockBack && !isDash)
            {
                Move(horizontalInput);
            }


            //PlayerJump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isDash && !isKnockBack)
            {
                Jump();
            }


            //PlayerUpAttack
            if (Input.GetKey(KeyCode.W))
            {
                attackPos.localPosition = new Vector3(0, 0.52f, attackPos.position.z);

                if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
                {
                    anim.SetTrigger("attackUp");

                }
            }
            //PlayerDownAttack
            else if (Input.GetKey(KeyCode.S))
            {
                attackPos.localPosition = new Vector3(0, -2f, attackPos.position.z);

                if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
                {
                    isDownAttack = true;
                    anim.SetTrigger("attackDown");

                }
            }
            //PlayerAttack
            else
            {
                attackPos.localPosition = new Vector3(1.12f, -0.13f, attackPos.position.z);
                if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
                {
                    anim.SetTrigger("Attack");
                }

            }


            //When player has no health
            if (playerHealthSc.health <= 0)
            {
                GameOver();
            }


            //PlayerDash
            if (Input.GetMouseButtonDown(1) && !dashCoolDown && !isKnockBack)
            {
                dashCoolDown = true;
                Dash();
            }


            //Player throw fireBall to left or right
            if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextAttackTime && fireBallCount > 0)
            {
                anim.SetTrigger("FireBall");
            }

        }
        else
        {
            rb.velocity = Vector2.zero;
            anim.SetInteger("AnimState", 0);
            playerSound.StopWalkSound();
        }

    }
    public void DownAttackFalse()
    {
        isDownAttack = false;
    }

    void Dash()
    {
        isDash = true;
        playerSound.DashSound();
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
        yield return new WaitForSeconds(playerDataSc.dashCoolDown);
        dashCoolDown = false;
    }
    void GameOver()
    {
        playerSound.DieSound();
        rb.velocity = Vector3.zero;
        Time.timeScale = .5f;
        Time.fixedDeltaTime *= .5f;
        isGameOver = true;
        anim.SetTrigger("Die");
        StartCoroutine(WaitGameOver());
    }
    IEnumerator WaitGameOver()
    {
        yield return new WaitForSeconds(.7f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(.5f);
        Time.timeScale = 1;
        Time.fixedDeltaTime = fixedTime;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    void UpAndDown(float vertical)
    {

        if (vertical >= .2f)
        {
            rb.velocity = new Vector2(rb.velocity.x, upSpeed);
            rb.gravityScale = firstGravityScale;
        }
        else if (vertical <= -.2f)
        {
            rb.velocity = new Vector2(rb.velocity.x, -upSpeed);
            rb.gravityScale = firstGravityScale;
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.gravityScale = 0;

        }
    }
    void FireBall()
    {
        playerSound.FireBallSound();
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y - .5f);
        nextAttackTime = Time.time + 1f / attackRate;
        fireBallPref.transform.localScale = transform.localScale;
        Instantiate(fireBallPref, playerPos, Quaternion.identity);
        fireBallCount--;
        canvasManager.FireBallText(fireBallCount);

    }
    void Attack()
    {
        nextAttackTime = Time.time + 1f / attackRate;
        bool isNull = true;

        // find enemies in range
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(1);
            playerSound.AttackEnemySound();
            isNull = false;
            if (isDownAttack && !isKnockBack)
            {
                rb.AddForce(Vector2.up * downAttackForce, ForceMode2D.Impulse);
            }
        }

        // find boss
        Collider2D[] bosses = Physics2D.OverlapCircleAll(attackPos.position, attackRange, LayerMask.GetMask("Boss"));
        foreach (Collider2D boss in bosses)
        {

            isNull = false;

            if (boss.gameObject.tag == "Boss" && boss != null)
            {
                boss.GetComponent<BossController>().TakeBossDamage(1);
                playerSound.AttackEnemySound();
                if (isDownAttack && !isKnockBack)
                {
                    rb.AddForce(Vector2.up * downAttackForce, ForceMode2D.Impulse);

                }

            }

        }

        // find chests in range
        Collider2D[] chests = Physics2D.OverlapCircleAll(attackPos.position, attackRange, LayerMask.GetMask("Chest"));
        foreach (Collider2D chest in chests)
        {
            isNull = false;

            // if chest is close, open it
            if (chest.GetComponent<Chest>().isOpen == false)
            {
                chest.GetComponent<Chest>().OpenChest();
            }
            // if chest is open, close it
            else
            {
                chest.GetComponent<Chest>().CloseChest();
            }
        }
        Collider2D[] secretPlace = Physics2D.OverlapCircleAll(attackPos.position, attackRange, LayerMask.GetMask("SecretPlace"));
        foreach (Collider2D place in secretPlace)
        {
            if (place != null)
            {
                place.GetComponent<SecretPlace>().DestroyPlace(-1);
                playerSound.WallHitSound();
                isNull = false;

            }
        }
        Collider2D[] spears = Physics2D.OverlapCircleAll(attackPos.position, attackRange, LayerMask.GetMask("Spear"));
        foreach (Collider2D spear in spears)
        {
            if (spear != null && isDownAttack && !isKnockBack)
            {
                rb.AddForce(Vector2.up * downAttackForce, ForceMode2D.Impulse);
                playerSound.AttackSpearSound();
                isNull = false;

            }
        }
        if (isNull)
        {
            playerSound.AttackSound();
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
    void Move(float horizontalInput)
    {
        if (isOnPlatform)
        {
            if (horizontalInput == 0)
            {
                rb.velocity = new Vector2(platformRb.velocity.x, rb.velocity.y);
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
            playerSound.StopWalkSound();
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
        playerSound.JumpSound();
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        anim.SetTrigger("Jump");
    }

    public void UpdateHealth(int add)
    {

        playerHealthSc.health += add;
        playerHealthSc.SetHealthImg();
    }
    public void KnockBack(Vector3 enemyPos, GameObject enemy)
    {
        if (playerHealthSc.health > 0)
        {
            playerSound.HurtSound();
            isKnockBack = true;
            anim.SetTrigger("Hurt");
            anim.SetBool("isHurt", true);

            Vector2 knockBackDir = transform.position - enemyPos;
            knockBackDir.Normalize();
            if (!isDownAttack)
            {
                rb.AddForce(knockBackDir * 3, ForceMode2D.Impulse);

            }

            Physics2D.IgnoreLayerCollision(gameObject.layer, enemy.layer);

            Time.timeScale = slowMotion;
            Time.fixedDeltaTime *= slowMotion;
            StartCoroutine(KnockBackTimer(enemy));
        }

    }
    public void KnockBackSpike(Vector3 enemyPos, GameObject enemy)
    {
        if (playerHealthSc.health > 0)
        {
            playerSound.HurtSound();
            isKnockBack = true;
            anim.SetTrigger("Hurt");
            anim.SetBool("isHurt", true);
            Time.timeScale = slowMotion;
            Time.fixedDeltaTime *= slowMotion;
            Physics2D.IgnoreLayerCollision(gameObject.layer, enemy.layer);
            StartCoroutine(KnockBackTimerSpike(enemyPos, enemy));
        }

    }



    IEnumerator KnockBackTimer(GameObject enemy)
    {
        yield return new WaitForSeconds(.02f);
        Time.timeScale = 1;
        Time.fixedDeltaTime = fixedTime;
        yield return new WaitForSeconds(.5f);
        isKnockBack = false;
        anim.SetBool("isHurt", false);
        yield return new WaitForSeconds(.8f);

        if (enemy != null)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, enemy.layer, false);
        }
    }
    IEnumerator KnockBackTimerSpike(Vector3 enemyPos, GameObject enemy)
    {
        Vector2 knockBackDir = transform.position - enemyPos;
        knockBackDir.Normalize();
        if (!isDownAttack)
        {
            rb.AddForce(knockBackDir * 5, ForceMode2D.Impulse);

        }
        yield return new WaitForSeconds(.02f);
        Time.timeScale = 1;
        Time.fixedDeltaTime = fixedTime;
        canvasManager.SpikeHurtActive();
        yield return new WaitForSeconds(0.7f);
        isKnockBack = false;
        anim.SetBool("isHurt", false);
        canvasManager.SpikeHurtInactive();
        transform.position = exitGroundPos;
        yield return new WaitForSeconds(.6f);
        if (enemy != null)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, enemy.layer, false);
        }

    }
    void Protection()
    {
        isProtection = true;
        protection.SetActive(true);
        Physics2D.IgnoreLayerCollision(7, 9);
        Physics2D.IgnoreLayerCollision(7, 6);
        Physics2D.IgnoreLayerCollision(7, 11);
        StartCoroutine(ProtectionTimer());
    }
    IEnumerator ProtectionTimer()
    {
        yield return new WaitForSeconds(5);
        IgnoreLayerFalse();
        protection.SetActive(false);
        isProtection = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            anim.SetBool("Grounded", true);

        }
        if (collision.gameObject.tag == "NextLevel")
        {
            if (isParshment)
            {
                Destroy(collision.gameObject);
                GameObject.FindGameObjectWithTag("BackSound").GetComponent<AudioSource>().Stop();
                playerSound.NextLevelSound();
                if (playerDataSc.currentScene <= SceneManager.GetActiveScene().buildIndex)
                    playerDataSc.currentScene += 1;
                Debug.Log(playerDataSc.currentScene);
                playerDataSc.Save();
                canvasManager.isNextLevelPanel = true;
                Time.timeScale = 0;


            }
            else
            {
                canvasManager.NextLevelText();
            }
        }
        if (collision.gameObject.tag == "Platform")
        {
            isOnPlatform = true;
            isGrounded = true;
            anim.SetBool("Grounded", true);
            platformRb = collision.gameObject.GetComponent<Rigidbody2D>();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            UpdateHealth(-1);
            KnockBack(collision.transform.position, collision.gameObject);
        }
        if (collision.gameObject.tag == "Spike")
        {
            UpdateHealth(-1);
            KnockBackSpike(collision.transform.position, collision.gameObject);

        }
        if (collision.gameObject.tag == "Laser")
        {
            UpdateHealth(-1);
            KnockBack(collision.transform.position, collision.gameObject);
        }
        if (collision.gameObject.tag == "Bullet")
        {
            UpdateHealth(-1);
            KnockBack(collision.transform.position, collision.gameObject);
        }
        if (collision.gameObject.tag == "Boss")
        {
            UpdateHealth(-1);
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
            CoinUpdate(1);
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Heal")
        {
            playerSound.HealthSound();
            if (playerHealthSc.health <= playerHealthSc.maxHealth - 1)
            {
                UpdateHealth(1);
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
                Protection();
            }
        }
        if (other.gameObject.tag == "parshment")
        {
            isParshment = true;
            playerSound.ParshmentSound();
            Destroy(other.gameObject);
            canvasManager.SetParshmentImage();
            if (canvasManager.isNextLevelText)
            {
                canvasManager.NextLevelTextInactive();
            }

        }
        if (other.gameObject.tag == "Ladder")
        {
            isInLadder = true;
            ladderGround.enabled = false;

        }
        if (other.gameObject.tag == "DeathArea")
        {
            UpdateHealth(-1);
            canvasManager.SpikeHurtActive();
            isKnockBack = true;
            if (playerHealthSc.health > 0)
            {
                StartCoroutine(DeathAreaTimer());
            }

        }
        if (other.gameObject.tag == "SafetyArea")
        {
            exitGroundPos = new Vector3(transform.position.x, other.gameObject.transform.position.y, transform.position.z);
        }

    }
    IEnumerator DeathAreaTimer()
    {

        yield return new WaitForSeconds(.2f);
        transform.position = exitGroundPos;
        yield return new WaitForSeconds(.7f);
        canvasManager.SpikeHurtInactive();
        isKnockBack = false;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            isInLadder = false;
            rb.gravityScale = firstGravityScale;
            anim.SetBool("Grounded", true);
            isGrounded = true;
            ladderGround.enabled = true;

        }
    }




}
