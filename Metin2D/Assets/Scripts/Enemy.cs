using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] EnemyMove enemyMove;
    [SerializeField] GameObject coinPrefab;
    float speed;

    public void TakeDamage(float damage)
    {
        health -= damage;
        speed = enemyMove.speed;
        enemyMove.speed = -enemyMove.speed * 3;
        StartCoroutine(ResetSpeed());
        Debug.Log("hit");
    }
    IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(.08f);
        enemyMove.speed = 0;
        yield return new WaitForSeconds(.25f);
        enemyMove.speed = speed;
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

}
