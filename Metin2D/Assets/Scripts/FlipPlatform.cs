using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipPlatform : MonoBehaviour
{
    [SerializeField] float coolDown;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag=="Player")
        {
            StartCoroutine(CoolDownTimer());
        }
    }
    IEnumerator CoolDownTimer()
    {
        yield return new WaitForSeconds(coolDown);
        anim.SetBool("isPlayer", true);
        yield return new WaitForSeconds(coolDown + 1f);
        anim.SetBool("isPlayer",false);
        
    }
}
