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
        yield return new WaitForSeconds(.7f);
        isStop = true;
        yield return new WaitForSeconds(.5f);
        isTimeUp = true;
        yield return new WaitForSeconds(.7f);
        Destroy(gameObject);


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isStop)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else if (isTimeUp && isStop)
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Destroy")
        {
            Destroy(gameObject);
        }
        if( other.gameObject.tag =="Wall")
        {
            isStop = true;
        }
    }
}
