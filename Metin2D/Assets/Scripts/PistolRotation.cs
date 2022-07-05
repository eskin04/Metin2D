using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolRotation : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] float circleRange;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawn;
    float coolDown;
    // Start is called before the first frame update
    void Start()
    {
        coolDown = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
        Collider2D player = Physics2D.OverlapCircle(transform.position, circleRange, LayerMask.GetMask("Player"));
        if(player!= null){
            Vector2 direction = playerPos.position - transform.position;
            direction.Normalize();
            transform.rotation= Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            if(Time.time>=coolDown){
                Instantiate(bullet, bulletSpawn.position, Quaternion.identity);
                coolDown = Time.time + 3;
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, circleRange);
    }
}
