using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool isOpen;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenChest(){
        isOpen = true;
        anim.SetBool("isOpen", true);
    }
    public void CloseChest(){
        isOpen = false;
        anim.SetBool("isOpen", false);
    }
}
