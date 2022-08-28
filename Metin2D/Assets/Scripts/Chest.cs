using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool isOpen;
    Animator anim;
    [SerializeField] GameObject item;
    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip closeSound;
    AudioSource Source;
    bool isFirstTime;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Source = GetComponent<AudioSource>();
        isFirstTime = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OpenChest()
    {
        isOpen = true;
        anim.SetBool("isOpen", true);
        Source.PlayOneShot(openSound);
        if (isFirstTime)
        {
            Instantiate(item, new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity);
            isFirstTime = false;

        }
    }
    public void CloseChest()
    {
        isOpen = false;
        anim.SetBool("isOpen", false);
        Source.PlayOneShot(closeSound);
    }
}
