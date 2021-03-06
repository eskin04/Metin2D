using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] GameObject player;
    [SerializeField] Transform bossGround;
    [SerializeField] Transform shootPoint;
    [SerializeField] float jumpForce;
    [SerializeField] float attackForce;
    [SerializeField] float waitTime;
    [SerializeField] float backForce;
    [SerializeField] float dashForce;
    [SerializeField] float health;
    [SerializeField] float arrowCount;
    [SerializeField] float arrowSpawnTime;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject fires;
    [SerializeField] HealthBar healthBar;
    SpriteRenderer spriteRenderer;
    Vector2 direction;
    float time;
    float localScaleX;
    float prevRandom;
    float powerTime;
    bool isPowerTime;
    float maxHealth;
    int prevArrow;
    List<string> attacks = new List<string>() { "jumpAttack", "dashAttack", "shootAttack" };

    // Start is called before the first frame update
    void Start()
    {
        maxHealth=health;
        powerTime=2;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        localScaleX = transform.localScale.x;
        direction = (player.transform.position - bossGround.position).normalized;
        time = Time.time  + 2;
        healthBar.SetMaxHealth(health);
        prevRandom=10;
    }

    // Update is called once per frame
    void Update()
    {
        direction = (player.transform.position - bossGround.position).normalized;
        if (Time.time >= time && !isPowerTime)
        {
            int random = Random.Range(0, 3);
            while (random == prevRandom)
            {
                random = Random.Range(0, 3);
            }
            rb.velocity = Vector2.zero;
            switch (attacks[random])
            {
                case "jumpAttack":
                    StartCoroutine(WaitJump());
                    time = Time.time + 4f;
                    break;
                case "dashAttack":
                    StartCoroutine(WaitDash());
                    time = Time.time + 3.5f;
                    break;
                case "shootAttack":
                    StartCoroutine(WaitShoot());
                    time = Time.time + 3.5f;
                    break;
            }
            
            prevRandom = random;
            
        }
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
        if (health <= maxHealth/2 && powerTime ==2)
        {
            powerTime-=1;
            StopAllCoroutines();
            StartCoroutine(WaitArrow());
            time = Time.time + 9f;
            isPowerTime=true;
        }

        if (health <= maxHealth/4 && powerTime ==1)
        {
            powerTime-=1;
            StopAllCoroutines();
            StartCoroutine(WaitArrow());
            time = Time.time + 9f;
            isPowerTime=true;
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
    }
    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
    IEnumerator WaitJump(){
        spriteRenderer.color = Color.blue;
        yield return new WaitForSeconds(.6f);
        JumpAttack();
    }
    IEnumerator WaitDash(){
        spriteRenderer.color = Color.green;
        yield return new WaitForSeconds(.6f);
        DashAttack();
    }
    IEnumerator WaitShoot(){
        spriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(.6f);
        ShootAttack();
    }
    IEnumerator WaitArrow()
    {
        spriteRenderer.color = Color.red;
        transform.position = new Vector3(-1.3f,8,0);
        rb.gravityScale=0;
        yield return new WaitForSeconds(.6f);
        ArrowAttack();
    }

    void JumpAttack()
    {
        StartCoroutine(ResetColor());
        ChangeDirection();
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        StartCoroutine(Attack());
    }
    IEnumerator Attack()
    {

        yield return new WaitForSeconds(waitTime);
        rb.velocity = direction * attackForce;
        yield return new WaitForSeconds(waitTime);
        BackForce();
    }
    void BackForce()
    {
        rb.AddForce(Vector2.left * backForce * direction, ForceMode2D.Impulse);

    }
    void ShootAttack()
    {
        StartCoroutine(ResetColor());
        ChangeDirection();
        Instantiate(bullet, shootPoint.position, transform.rotation);

    }
    void ArrowAttack()
    {
        fires.SetActive(true);
        StartCoroutine(ResetColor());
        ChangeDirection();
        StartCoroutine(SpawnArrow());
        StartCoroutine(SetHealthMax());
    }
    IEnumerator SetHealthMax()
    {
        for (int i = 0; i < maxHealth/2; i++)
        {
            health +=1;
            healthBar.SetHealth(health);
            yield return new WaitForSeconds(.5f);

        }
    }
    IEnumerator SpawnArrow()
    {
        for (int i = 0; i < arrowCount; i++)
        {
            int posX = Random.Range(-15,12);
            yield return new WaitForSeconds(arrowSpawnTime);
            while(posX>=prevArrow-1 && posX<=prevArrow+1)
            {
                posX = Random.Range(-15,12);
            }
            Instantiate(arrow,new Vector2(posX,18),arrow.transform.rotation);
            prevArrow=posX;

        }
        fires.SetActive(false);
        rb.gravityScale=1;
        isPowerTime = false;
    }
    void DashAttack()
    {
        StartCoroutine(ResetColor());
        ChangeDirection();
        rb.AddForce(Vector2.right * direction * dashForce, ForceMode2D.Impulse);
        StartCoroutine(DAttack());
    }
    IEnumerator DAttack()
    {
        yield return new WaitForSeconds(waitTime);
        rb.velocity = Vector2.zero;
        ChangeDirection();
        yield return new WaitForSeconds(waitTime);
        BackForce();
    }
    void ChangeDirection()
    {
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            rb.velocity = Vector2.zero;
            ChangeDirection();

        }
        if(other.gameObject.tag =="Wall"){
            rb.velocity = new Vector2(0,-15f);
            ChangeDirection();
        }
    }

}
