using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearMove : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float timer;
    bool coolDown;
    bool oneTime;
    // Start is called before the first frame update
    void Start()
    {
        
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!coolDown)
        {
            transform.Translate(Vector2.right*speed*Time.deltaTime);
            if(!oneTime)
            {    
                oneTime=true;
                StartCoroutine(coolDownTimer());
            }
        }

    }
    IEnumerator coolDownTimer()
    {
        yield return new WaitForSeconds(timer);
        coolDown=true;
    }
}
