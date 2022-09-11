using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip attackEnemySound;
    [SerializeField] AudioClip attackSpearSound;
    [SerializeField] AudioClip wallHitSound;
    [SerializeField] AudioClip dashSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip coinSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip fireBall;
    [SerializeField] AudioClip shopSound;
    [SerializeField] AudioClip healthSound;
    [SerializeField] AudioClip nextLevelSound;
    [SerializeField] AudioClip parshmentSound;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource audioSourceWalk;



    public void AttackSound()
    {
        audioSource.pitch = .5f;
        audioSource.PlayOneShot(attackSound, .3f);
    }
    public void JumpSound()
    {
        audioSource.pitch = 1.2f;
        audioSource.PlayOneShot(jumpSound);

    }
    public void HurtSound()
    {
        audioSource.pitch = 1.1f;
        audioSource.PlayOneShot(hurtSound, .3f);

    }
    public void DieSound()
    {
        audioSource.pitch = 1.1f;
        audioSource.PlayOneShot(deathSound, .5f);

    }
    public void AttackEnemySound()
    {
        audioSource.pitch = 1.2f;
        audioSource.PlayOneShot(attackEnemySound);
    }
    public void AttackSpearSound()
    {
        audioSource.pitch = .5f;
        audioSource.PlayOneShot(attackSpearSound, .3f);
    }
    public void WallHitSound()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(wallHitSound);
    }
    public void DashSound()
    {
        audioSource.pitch = .5f;
        audioSource.PlayOneShot(dashSound, .3f);
    }
    public void WalkSound()
    {
        audioSourceWalk.Play();
    }
    public void StopWalkSound()
    {
        audioSourceWalk.Stop();
    }
    public void CoinSound()
    {
        audioSource.pitch = 1.2f;
        audioSource.PlayOneShot(coinSound, .3f);
    }
    public void FireBallSound()
    {
        audioSource.pitch = .9f;
        audioSource.PlayOneShot(fireBall, .5f);
    }
    public void ShopSound()
    {
        audioSource.pitch = 1.2f;
        audioSource.PlayOneShot(shopSound, .5f);
    }
    public void HealthSound()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(healthSound, .5f);
    }
    public void NextLevelSound()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(nextLevelSound, .8f);
    }
    public void ParshmentSound()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(parshmentSound, .5f);
    }

}
