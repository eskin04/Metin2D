using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBall : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D rb;
    [SerializeField] float deathTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, deathTime);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(speed,rb.velocity.y);

    }
}
