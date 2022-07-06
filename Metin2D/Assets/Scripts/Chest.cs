using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool isOpen;
    Animator anim;
    [SerializeField] GameObject item;
    bool isFirstTime;
    // Start is called before the first frame update
    void Start()
    {
        anim= GetComponent<Animator>();
        isFirstTime = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenChest(){
        isOpen = true;
        anim.SetBool("isOpen", true);
        if(isFirstTime)
        {
            Instantiate(item, new Vector2(transform.position.x,transform.position.y+1), Quaternion.identity);
            isFirstTime = false;
        }
    }
    public void CloseChest(){
        isOpen = false;
        anim.SetBool("isOpen", false);
    }
}
