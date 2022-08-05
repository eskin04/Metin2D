using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject deathPrefab;
    public float enemiesSpeed;
    public float firstSpeed;
    bool isDead;
    SpriteRenderer sprite;
    Color firstColor;
    Color colorRed;
    float firstLight;
    float hitLight;
    Light2D childLight;
    private void Start()
    {
        firstSpeed = enemiesSpeed;
        sprite=GetComponent<SpriteRenderer>();
        childLight = transform.Find("Light 2D").GetComponent<Light2D>();
        firstLight = childLight.intensity;
        hitLight = firstLight*3;
        firstColor = childLight.color;
        colorRed = new Color32(97, 7, 10, 255);

    }

    public void TakeDamage(float damage)
    {
        
        health -= damage;
        Transform playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Vector3 direct = (playerPos.position - transform.position).normalized;
        enemiesSpeed = enemiesSpeed * 1.5f * transform.localScale.x*direct.x;
        StartCoroutine(ChangeColor());
    }
    IEnumerator ChangeColor()
    {
        childLight.intensity = hitLight;
        childLight.color = colorRed;
        for (int i = 0; i < 3; i++)
        {

            yield return new WaitForSeconds(.05f);
            childLight.intensity = hitLight;
            childLight.color = colorRed;
            yield return new WaitForSeconds(.05f);
            childLight.intensity = firstLight;
            childLight.color = firstColor;

        }
        enemiesSpeed = 0;
        yield return new WaitForSeconds(.25f);
        enemiesSpeed = firstSpeed;
    }
    public void HitPlayer()
    {
        Transform playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Vector3 direct = (playerPos.position - transform.position).normalized;
        enemiesSpeed = enemiesSpeed * 1.5f * transform.localScale.x * direct.x;
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
            HitPlayer();
        }
    }

}
