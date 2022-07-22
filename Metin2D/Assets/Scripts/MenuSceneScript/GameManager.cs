using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Time.timeScale = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        menuPanel.SetActive(false);
        anim.SetTrigger("isStart");
        StartCoroutine(StartGameAfterDelay());
        
    }
    IEnumerator StartGameAfterDelay()
    {
        yield return new WaitForSeconds(1);
        anim.enabled = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
