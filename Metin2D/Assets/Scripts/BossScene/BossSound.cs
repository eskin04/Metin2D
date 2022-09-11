using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSound : MonoBehaviour
{
    [SerializeField] AudioClip introSound;
    [SerializeField] AudioClip dieSound;
    [SerializeField] AudioClip powerSound;
    [SerializeField] AudioClip powerSound2;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip backForce;
    [SerializeField] AudioClip victorySound;
    [SerializeField] CanvasManager canvasManager;
    [SerializeField] AudioClip[] attackSound;

    AudioSource source;
    [SerializeField] AudioSource music;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void FightSound()
    {
        music.Play();
    }
    public void StopFightSound()
    {
        music.Stop();
    }

    public void IntroSound()
    {
        source.PlayOneShot(introSound, .6f);
    }
    public void BossDieSound()
    {
        source.PlayOneShot(dieSound, .5f);
        foreach (GameObject sound in GameObject.FindGameObjectsWithTag("BackSound"))
        {
            sound.GetComponent<AudioSource>().Stop();
        }
    }
    public void AttackSound1()
    {
        source.PlayOneShot(attackSound[0], .5f);
    }
    public void AttackSound2()
    {
        source.PlayOneShot(attackSound[1], .5f);
    }
    public void AttackSound3()
    {
        source.PlayOneShot(attackSound[2], .5f);
    }
    public void ArrowAttackSound()
    {
        source.PlayOneShot(powerSound, .2f);
    }
    public void ArrowAttackSound2()
    {
        source.PlayOneShot(powerSound2, .5f);
    }
    public void HurtSound()
    {
        source.PlayOneShot(hurtSound, .3f);
    }
    public void BackForceSound()
    {
        source.PlayOneShot(backForce, .5f);
    }
    public void VictorySound()
    {
        source.PlayOneShot(victorySound);
        canvasManager.Victory();

    }
}
