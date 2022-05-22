using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform attackPos;
    [SerializeField] float attackRange;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float attackRate;
    [SerializeField] Animator anim;
    int health = 5;
    bool isGrounded = true;
    float nextAttackTime;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("AirSpeed", rb.velocity.y);
        float horizontalInput = Input.GetAxis("Horizontal");
        Move(horizontalInput);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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

    }
    void GameOver()
    {
        Debug.Log("Game Over");
    }
    void Attack()
    {
        nextAttackTime = Time.time + 1f / attackRate;
        anim.SetTrigger("Attack");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(1);
        }
    }
    void Move(float horizontalInput)
    {

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
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
            transform.localScale = new Vector3(1.5f, 1.5f, 1);
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1);
        }
    }
    void Jump()
    {
        isGrounded = false;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        anim.SetTrigger("Jump");
    }

    void UpdateHealth(int add)
    {
        health += add;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            anim.SetBool("Grounded", true);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            anim.SetTrigger("Hurt");
            UpdateHealth(-1);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            anim.SetBool("Grounded", false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
        }
    }

}
