using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBomb : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] float waitTime;
    [SerializeField] GameObject explode;
    SpriteRenderer sprite;
    Collider2D hit;
    Collider2D hitEnemy;
    bool coolDown;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Player"));
        hitEnemy  = Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Enemy"));
        if (!coolDown)
        {
            
            StartCoroutine(Explode());
        }

    }
    IEnumerator Explode()
    {
        coolDown = true;
        yield return new WaitForSeconds(waitTime);
        if(hit!=null)
        {
           hit.GetComponent<PlayerController>().UpdateHealth(-2);
           hit.GetComponent<PlayerController>().KnockBack(transform.position,gameObject);
        }
        if(hitEnemy!= null)
        {
            hitEnemy.GetComponent<Enemy>().TakeDamage(2);
        }
        explode.SetActive(true);
        sprite.enabled = false;
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
