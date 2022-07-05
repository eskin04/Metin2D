using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject deathPrefab;
    public float enemiesSpeed;
    float firstSpeed;
    bool isDead;
    SpriteRenderer sprite;
    private void Start()
    {
        firstSpeed = enemiesSpeed;
        sprite=GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("hit");
        health -= damage;
        enemiesSpeed = -enemiesSpeed * 1.5f;
        StartCoroutine(ResetSpeed());

    }
    IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(.25f);
        enemiesSpeed = 0;
        yield return new WaitForSeconds(.25f);
        enemiesSpeed = firstSpeed;
    }
    private void Update()
    {
        if (health <= 0 && !isDead)
        {
            KillEnemy();
        }
    }
    private void KillEnemy()
    {
        isDead=true;
        Vector2 coinPos = new Vector2(transform.position.x, transform.position.y + 1);
        sprite.enabled=false;
        GameObject deathClone=Instantiate(deathPrefab, transform.position, Quaternion.identity);
        StartCoroutine(SpawnCoin(coinPos,deathClone));
    }
    IEnumerator SpawnCoin(Vector2 coinPos,GameObject deathClone)
    {
        yield return new WaitForSeconds(.25f);
        Destroy(deathClone);
        Instantiate(coinPrefab, coinPos, Quaternion.identity);
        Destroy(gameObject);
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            TakeDamage(0);
        }
    }

}
