using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] GameObject coinPrefab;
    public float enemiesSpeed;
    float firstSpeed;
    private void Start()
    {
        firstSpeed = enemiesSpeed;
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
        if (health <= 0)
        {
            KillEnemy();
        }
    }
    private void KillEnemy()
    {
        Vector2 coinPos = new Vector2(transform.position.x, transform.position.y + 1);
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
