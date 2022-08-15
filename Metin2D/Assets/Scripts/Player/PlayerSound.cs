using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip hurtSound;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void AttackSound()
    {
        audioSource.PlayOneShot(attackSound);
    }
    public void JumpSound()
    {
        audioSource.PlayOneShot(jumpSound);

    }
    public void HurtSound()
    {
        audioSource.PlayOneShot(hurtSound);

    }
}
