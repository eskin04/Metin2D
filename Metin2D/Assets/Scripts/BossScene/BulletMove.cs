using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] Transform Player;
    [SerializeField] float speed;
    [SerializeField] AudioClip throwSound;
    AudioSource source;
    GameObject weapon;
    bool isTimeUp;
    bool isStop;
    Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.PlayOneShot(throwSound, .5f);
        weapon = GameObject.Find("Weapon");
        weapon.SetActive(false);
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
        source.PlayOneShot(throwSound, .5f);
        yield return new WaitForSeconds(.7f);
        Destroy(gameObject);
        weapon.SetActive(true);


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
            weapon.SetActive(true);
        }
        if (other.gameObject.tag == "Wall")
        {
            isStop = true;
        }
    }
}
