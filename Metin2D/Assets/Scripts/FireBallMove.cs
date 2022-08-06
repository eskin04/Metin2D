using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallMove : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3);
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.right*speed*gameObject.transform.localScale.x;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "SecretPlace")
        {
            collision.gameObject.GetComponent<SecretPlace>().DestroyPlace(-2);
            Destroy(gameObject);
        }
        if(collision.gameObject.tag=="Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(1);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Boss")
        {
            collision.gameObject.GetComponent<BossController>().TakeDamage(1);
            Destroy(gameObject);
        }

    }
}
