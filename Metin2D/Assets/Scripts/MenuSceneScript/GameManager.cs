using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject player;
    [SerializeField] GameObject newGameButton;
    [SerializeField] GameObject gamePanel;
    [SerializeField] TextMeshProUGUI startText;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Time.timeScale = 0;
        Data data = SystemSave.LoadPlayer();
        if (data != null)
        {
            newGameButton.SetActive(true);
            startText.text = "Load Game";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        Time.timeScale = 1;
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        player.SetActive(true);
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
