using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] Sprite soundOn, soundOff;
    [SerializeField] Image soundButtonImg;
    PlayerController playerSc;

    bool isPause;
    void Start()
    {
        playerSc = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        playerSc.enabled = true;
        isPause = false;
    }
    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        playerSc.enabled = false;
        isPause = true;
    }
    public void Restart()
    {
        Time.timeScale = 1;
        playerSc.canvasManager.SpikeHurtActive();
        GameObject.FindGameObjectWithTag("BackSound").GetComponent<AudioSource>().Stop();
        StartCoroutine(RestartAfterDelay());

    }
    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        playerSc.canvasManager.SpikeHurtActive();
        GameObject.FindGameObjectWithTag("BackSound").GetComponent<AudioSource>().Stop();
        StartCoroutine(MainMenuAfterDelay());

    }
    IEnumerator MainMenuAfterDelay()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(0);
    }
    public void ChangeSound()
    {
        if (AudioListener.volume != 0)
        {
            AudioListener.volume = 0;
            soundButtonImg.sprite = soundOff;
        }
        else
        {
            AudioListener.volume = 1;
            soundButtonImg.sprite = soundOn;
        }
    }
}
