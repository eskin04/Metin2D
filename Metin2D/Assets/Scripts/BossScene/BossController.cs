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
    [SerializeField] GameObject bullet;
    SpriteRenderer spriteRenderer;
    Vector2 direction;
    float time;
    float localScaleX;
    float prevRandom;
    List<string> attacks = new List<string>() { "jumpAttack", "dashAttack", "shootAttack" };

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        localScaleX = transform.localScale.x;
        direction = (player.transform.position - bossGround.position).normalized;
        time = 2;
    }

    // Update is called once per frame
    void Update()
    {
        direction = (player.transform.position - bossGround.position).normalized;
        if (Time.time >= time)
        {
            int random = Random.Range(0, 3);
            while (random == prevRandom)
            {
                random = Random.Range(0, 3);
            }
            switch (attacks[random])
            {
                case "jumpAttack":
                    JumpAttack();
                    break;
                case "dashAttack":
                    DashAttack();
                    break;
                case "shootAttack":
                    ShootAttack();
                    break;
            }
            time = Time.time + 3;
            prevRandom = random;
        }
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        spriteRenderer.color = Color.red;
        StartCoroutine(ResetColor());

    }
    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    void JumpAttack()
    {
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
        yield return new WaitForSeconds(.5f);
        rb.velocity = Vector2.zero;
    }
    void BackForce()
    {
        rb.AddForce(Vector2.left * backForce * direction, ForceMode2D.Impulse);

    }
    void ShootAttack()
    {
        ChangeDirection();
        Instantiate(bullet, shootPoint.position, Quaternion.identity);

    }
    void DashAttack()
    {
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
        yield return new WaitForSeconds(.5f);
        rb.velocity = Vector2.zero;
    }
    void ChangeDirection()
    {
        if (direction.x > 0)
        {
            transform.localScale = new Vector2(-localScaleX, transform.localScale.y);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector2(localScaleX, transform.localScale.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            rb.velocity = Vector2.zero;
            ChangeDirection();

        }
    }
}
