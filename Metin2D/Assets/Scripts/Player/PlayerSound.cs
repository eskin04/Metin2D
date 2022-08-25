using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip attackEnemySound;
    [SerializeField] AudioClip attackSpearSound;
    [SerializeField] AudioClip dashSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioSource audioSourceLow;
    [SerializeField] AudioSource audioSourceHigh;
    [SerializeField] AudioSource audioSourceWalk;

    public void AttackSound()
    {
        audioSourceLow.PlayOneShot(attackSound);
    }
    public void JumpSound()
    {
        audioSourceHigh.PlayOneShot(jumpSound);

    }
    public void HurtSound()
    {
        audioSourceHigh.PlayOneShot(hurtSound);

    }
    public void AttackEnemySound()
    {
        audioSourceHigh.PlayOneShot(attackEnemySound);
    }
    public void AttackSpearSound()
    {
        audioSourceLow.PlayOneShot(attackSpearSound);
    }
    public void DashSound()
    {
        audioSourceLow.PlayOneShot(dashSound);
    }
    public void WalkSound()
    {
        audioSourceWalk.Play();
    }
    public void StopWalkSound()
    {
        audioSourceWalk.Stop();
    }
}
