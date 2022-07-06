using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    [SerializeField] float maxRange;
    [SerializeField] float speed;
    [SerializeField] float minRange;
    Rigidbody2D rb;
    float direction;
    // Start is called before the first frame update
    void Start()
    {
        direction = 1;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, maxRange+1f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            if (hit.distance >= maxRange)
            {
                direction = -1;
            }
            else if(hit.distance <= minRange)
            {
                direction=1;

            }
        }

        Debug.DrawRay(transform.position, Vector2.left * (maxRange+1), Color.red);

    }
}
