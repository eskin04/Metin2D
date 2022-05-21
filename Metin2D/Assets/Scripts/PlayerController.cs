using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] Animator anim;
    bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Move(horizontalInput);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();

        }
        anim.SetFloat("AirSpeed", rb.velocity.y);

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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            anim.SetBool("Grounded", true);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            anim.SetBool("Grounded", false);
        }
    }
}
