using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManAnim : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void JumpForce()
    {
        anim.SetTrigger("JumpF");
    }
    public void FrontForce()
    {
        anim.SetTrigger("FrontF");
    }
    public void SwordForce()
    {
        anim.SetTrigger("SwordF");
    }
    public void GroundTrue()
    {
        anim.SetBool("isGround", true);
    }
    public void GroundFalse()
    {
        anim.SetBool("isGround", false);
    }
    public void BackWalk()
    {
        anim.SetBool("isWalk", true);
    }
    public void EndBackWalk()
    {
        anim.SetBool("isWalk", false);
    }
    public void ArrowPowerStart()
    {
        anim.SetBool("isArrowPower", true);
        anim.SetTrigger("ArrowAttack");
    }
    public void ArrowPowerEnd()
    {
        anim.SetBool("isArrowPower", false);
    }
    public void SwordAttackAnim()
    {
        anim.SetTrigger("swordAttack");
    }
    public void BossDie()
    {
        anim.SetTrigger("die");
    }
    public void BossHurt()
    {
        anim.SetBool("isHurt", true);
    }
    public void BossHurtEnd()
    {
        anim.SetBool("isHurt", false);
    }
}
