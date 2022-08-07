using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallMove : MonoBehaviour
{
    [SerializeField] float speed;
    GameObject explosion;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    bool isDestroy;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        Destroy(gameObject, 3);
        explosion = transform.Find("explosion").gameObject;
    }
    void DestroyObject()
    {
        isDestroy = true;
        rb.velocity = Vector2.zero;
        sprite.enabled = false;
        explosion.SetActive(true);
        StartCoroutine(DestroyTime());
        
    }
    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDestroy)
        {
            rb.velocity = Vector2.right * speed * gameObject.transform.localScale.x;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Wall")
        {
            DestroyObject();
        }
        if (collision.gameObject.tag == "SecretPlace")
        {
            collision.gameObject.GetComponent<SecretPlace>().DestroyPlace(-2);
            DestroyObject();
        }
        if(collision.gameObject.tag=="Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(1);
            DestroyObject();

        }
        if (collision.gameObject.tag == "Boss")
        {
            collision.gameObject.GetComponent<BossController>().TakeBossDamage(1);
            DestroyObject();
        }

    }
}
