using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombMove : MonoBehaviour
{
    Rigidbody2D rb;
    Transform attackPos;
    PlayerController playerScript;
    [SerializeField] float attackRange;
    [SerializeField] GameObject explodeParticle;

    Collider2D player;
    Collider2D enemy;
    bool coolDown;
    // Start is called before the first frame update
    void Start()
    {
            rb= GetComponent<Rigidbody2D>();
        attackPos = GameObject.Find("PlayerHead").transform;
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        

    }

    // Update is called once per frame
    void Update()
    {
        player = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Player"));
        enemy = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Enemy"));
        Vector3 direction= attackPos.position - transform.position;
        direction.Normalize();
        rb.velocity = direction*3;
        
        if(!coolDown)
        {
            StartCoroutine(coolDownTimer());
        }
        

    }
    IEnumerator coolDownTimer()
    {
        coolDown = true;
        yield return new WaitForSeconds(2);
        if(player!=null){
            playerScript.UpdateHealth(-2);
            playerScript.KnockBack(transform.position,gameObject);
        }
        if(enemy!=null)
        {
            enemy.GetComponent<Enemy>().TakeDamage(2);
        }
        explodeParticle.SetActive(true);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(.5f);
        if(gameObject!=null)
        {
            Destroy(gameObject);
        }
        
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag=="Player"){
            playerScript.UpdateHealth(-2);
            playerScript.KnockBack(transform.position,gameObject);
            explodeParticle.SetActive(true);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(hitCoolDown());
        }
    }
    IEnumerator hitCoolDown()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }

}
