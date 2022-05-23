using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float maxDistance;
    [SerializeField] bool isRightDirection = true;
    Enemy enemy;
    int rightDirection;
    public int direction;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();

        if (isRightDirection)
        {
            rightDirection = 1;
            direction = 1;
        }
        else if (!isRightDirection)
        {
            rightDirection = -1;
            direction = -1;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        rb.velocity = new Vector2(direction * enemy.enemiesSpeed, rb.velocity.y);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * rightDirection, 10f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position, Vector2.right * maxDistance * rightDirection, Color.red);
        if (hit.collider != null)
        {
            if (hit.distance <= 1)
            {
                direction = -1;
            }
            else if (hit.distance >= maxDistance)
            {
                direction = 1;
            }
        }
    }
}
