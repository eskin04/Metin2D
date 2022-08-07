using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] GameObject player;
    [SerializeField] Transform bossGround;
    [SerializeField] Transform shootPoint;
    [SerializeField] float jumpForce;
    [SerializeField] float attackForce;
    [SerializeField] float waitTime;
    [SerializeField] float backForce;
    [SerializeField] float dashForce;
    [SerializeField] float health;
    [SerializeField] float arrowCount;
    [SerializeField] float arrowSpawnTime;
    [SerializeField] float swordJump;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject arrow;
    [SerializeField] HealthBar healthBar;
    Light2D lightBoss;
    float firstLight;
    SwordManAnim swordManScript;
    Animator anim;
    Vector2 direction;
    float time;
    float localScaleX;
    float prevRandom;
    float powerTime;
    bool isPowerTime;
    bool isBossDie;
    float maxHealth;
    int prevArrow;
    List<string> attacks = new List<string>() { "jumpAttack", "dashAttack", "shootAttack","swordAttack" };

    // Start is called before the first frame update
    void Start()
    {
        maxHealth=health;
        powerTime=2;
        rb = GetComponent<Rigidbody2D>();
        localScaleX = transform.localScale.x;
        direction = (player.transform.position - bossGround.position).normalized;
        time = Time.time  + 2;
        healthBar.SetMaxHealth(health);
        prevRandom=10;
        anim = GetComponent<Animator>();
        swordManScript = transform.Find("sword_man").GetComponent<SwordManAnim>();
        lightBoss = transform.Find("Light 2D").GetComponent<Light2D>();
        firstLight = lightBoss.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        direction = (player.transform.position - bossGround.position).normalized;
        if (Time.time >= time && !isPowerTime && !isBossDie)
        {
            int random = Random.Range(0, attacks.Count);
            while (random == prevRandom)
            {
                random = Random.Range(0, attacks.Count);
            }
            rb.velocity = Vector2.zero;
            switch (attacks[random])
            {
                case "jumpAttack":
                    StartCoroutine(WaitJump());
                    time = Time.time + 4.5f;
                    break;
                case "dashAttack":
                    StartCoroutine(WaitDash());
                    time = Time.time + 4f;
                    break;
                case "shootAttack":
                    StartCoroutine(WaitShoot());
                    time = Time.time + 4f;
                    break;
                case "swordAttack":
                    StartCoroutine(WaitSword());
                    time = Time.time + 2f;
                    break;
            }
            
            prevRandom = random;
            
        }
        if (health <= 0 && !isBossDie)
        {
            KillBoss();
        }
        if (health <= maxHealth/2 && powerTime ==2)
        {
            powerTime-=1;
            StopAllCoroutines();
            StartCoroutine(WaitArrow());
            time = Time.time + 9f;
            isPowerTime=true;
        }

        if (health <= maxHealth/4 && powerTime ==1)
        {
            powerTime-=1;
            StopAllCoroutines();
            StartCoroutine(WaitArrow());
            time = Time.time + 9f;
            isPowerTime=true;
        }
    }
    void KillBoss()
    {
        StopAllCoroutines();
        StartCoroutine(LightTimer());
        isBossDie = true;
        if(rb.bodyType == RigidbodyType2D.Static)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;

        }
        swordManScript.BossDie();
        StartCoroutine(KillBossWait());
    }
    IEnumerator KillBossWait()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);

    }
    public void TakeBossDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        lightBoss.intensity = 5;
        StartCoroutine(LightTimer());
    }
    IEnumerator LightTimer()
    {
        yield return new WaitForSeconds(.1f);
        lightBoss.intensity = firstLight;
    }
    IEnumerator WaitSword()
    {
        swordManScript.SwordAttackAnim();
        rb.velocity = new Vector2(direction.x * swordJump, swordJump);
        yield return new WaitForSeconds(1f);
        SwordAttack();

    }
    IEnumerator WaitJump(){
        swordManScript.JumpForce();
        yield return new WaitForSeconds(1f);
        JumpAttack();
    }
    IEnumerator WaitDash(){
        swordManScript.FrontForce();
        yield return new WaitForSeconds(1f);
        DashAttack();
    }
    IEnumerator WaitShoot(){
        swordManScript.SwordForce();
        yield return new WaitForSeconds(1f);
        ShootAttack();
    }
    IEnumerator WaitArrow()
    {
        swordManScript.ArrowPowerStart();
        lightBoss.intensity = firstLight;
        transform.position = new Vector3(-1.3f,6,0);
        rb.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(.6f);
        ArrowAttack();

    }
    void SwordAttack()
    {
        ChangeDirection();
        rb.velocity = new Vector2(direction.x * swordJump , -swordJump);
        StartCoroutine(ZeroVelocity());
    }
    IEnumerator ZeroVelocity()
    {
        yield return new WaitForSeconds(waitTime);
        rb.velocity = Vector2.zero;


    }
    void JumpAttack()
    {
      
        ChangeDirection();
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        StartCoroutine(Attack());
    }
    IEnumerator Attack()
    {

        yield return new WaitForSeconds(waitTime);
        rb.velocity = direction * attackForce;
        yield return new WaitForSeconds(waitTime);
        BackForce();

    }
    void BackForce()
    {
        rb.AddForce(Vector2.left * backForce * direction, ForceMode2D.Impulse);
        swordManScript.BackWalk();
        StartCoroutine(EndBackWalk());
    }
    IEnumerator EndBackWalk()
    {
        yield return new WaitForSeconds(waitTime+.5f);
        swordManScript.EndBackWalk();
    }
    void ShootAttack()
    {
        
        ChangeDirection();
        Instantiate(bullet, shootPoint.position, transform.rotation);

    }
    void ArrowAttack()
    {
        ChangeDirection();
        StartCoroutine(SpawnArrow());
        StartCoroutine(SetHealthMax());
    }
    IEnumerator SetHealthMax()
    {
        for (int i = 0; i < maxHealth/2; i++)
        {
            health +=1;
            healthBar.SetHealth(health);
            yield return new WaitForSeconds(.5f);

        }
    }
    IEnumerator SpawnArrow()
    {
        for (int i = 0; i < arrowCount; i++)
        {
            int posX = Random.Range(-15,12);
            yield return new WaitForSeconds(arrowSpawnTime);
            while(posX>=prevArrow-1 && posX<=prevArrow+1)
            {
                posX = Random.Range(-15,12);
            }
            Instantiate(arrow,new Vector2(posX,18),arrow.transform.rotation);
            prevArrow=posX;

        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        isPowerTime = false;
        swordManScript.ArrowPowerEnd();
    }
    void DashAttack()
    {
        
        ChangeDirection();
        rb.AddForce(Vector2.right * direction * dashForce, ForceMode2D.Impulse);
        StartCoroutine(DAttack());
    }
    IEnumerator DAttack()
    {
        yield return new WaitForSeconds(waitTime);
        rb.velocity = Vector2.zero;
        ChangeDirection();
        yield return new WaitForSeconds(waitTime);
        BackForce();
    }
    void ChangeDirection()
    {
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            rb.velocity = Vector2.zero;
            ChangeDirection();
            swordManScript.GroundTrue();

        }
        if(other.gameObject.tag =="Wall"){
            rb.velocity = new Vector2(0,-15f);
            ChangeDirection();
            swordManScript.EndBackWalk();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if ( collision.gameObject.tag == "Ground")
        {
            swordManScript.GroundFalse();
        }
    }

}
