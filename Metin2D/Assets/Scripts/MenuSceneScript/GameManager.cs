using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject player;
    [SerializeField] GameObject newGameButton;
    [SerializeField] GameObject gamePanel;

    [SerializeField] GameObject optionsPanel;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Slider volumeSlider;
    [SerializeField] GameObject levelsPanel;
    [SerializeField] TextMeshProUGUI startText;
    [SerializeField] AudioSource background;
    [SerializeField] AudioSource menuMusic;
    [SerializeField] AudioClip buttonSound;
    [SerializeField] AudioClip clickSound;

    GameObject[] levelButtons;
    AudioSource audioSource;
    int currentScene;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        resolutionDropdown.value = QualitySettings.GetQualityLevel();
        volumeSlider.value = AudioListener.volume;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 0;
        Data data = SystemSave.LoadPlayer();
        if (data != null)
        {
            newGameButton.SetActive(true);
            startText.text = "Load Game";
            currentScene = data.sceneBuild;
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        menuMusic.Pause();
        background.Play();
        mainPanel.SetActive(false);
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
    void SetLevelButtonDisabled()
    {
        levelButtons = GameObject.FindGameObjectsWithTag("LevelButton");

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > currentScene)
            {
                levelButtons[i].GetComponent<Button>().interactable = false;
                levelButtons[i].GetComponent<EventTrigger>().enabled = false;
            }
        }
    }
    public void LoadLevelButton(int level)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(level);
    }
    public void OptionsMenu()
    {
        menuPanel.SetActive(false);
        optionsPanel.SetActive(true);

    }
    public void LevelsMenu()
    {
        menuPanel.SetActive(false);
        levelsPanel.SetActive(true);
        SetLevelButtonDisabled();
    }
    public void BackToMenu()
    {
        menuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        levelsPanel.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(resolutionDropdown.value);

    }
    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }
    public void SoundButtons()
    {
        audioSource.PlayOneShot(buttonSound, .2f);
    }
    public void ClickSound()
    {
        audioSource.PlayOneShot(clickSound, .5f);
    }

}
