using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] Transform Player;
    [SerializeField] float speed;
    bool isTimeUp;
    bool isStop;
    Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = (Player.position - transform.position).normalized;
        StartCoroutine(TimeUp());
    }
    IEnumerator TimeUp()
    {
        yield return new WaitForSeconds(.6f);
        rb.velocity = Vector2.zero;
        isStop = true;
        yield return new WaitForSeconds(.5f);
        isTimeUp = true;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isStop)
        {
            rb.velocity = new Vector2(direction.x * speed, 0);
        }
        else if (isTimeUp && isStop)
        {
            rb.velocity = new Vector2(-direction.x * speed, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Destroy")
        {
            Destroy(gameObject);
        }
    }
}
