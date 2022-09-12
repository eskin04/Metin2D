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
    [SerializeField] GameObject spikeHurt;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Slider volumeSlider;
    [SerializeField] GameObject levelsPanel;

    [SerializeField] GameObject tutorialPanel;
    [SerializeField] Transform tutorialTrigger;

    [SerializeField] TextMeshProUGUI startText;
    [SerializeField] AudioSource background;
    [SerializeField] AudioSource menuMusic;
    [SerializeField] AudioClip buttonSound;
    [SerializeField] AudioClip clickSound;
    [SerializeField] CanvasManager canvasManager;


    [SerializeField] GameObject[] levelButtons;
    AudioSource audioSource;
    int currentScene;
    Animator anim;
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        resolutionDropdown.value = QualitySettings.GetQualityLevel();
        volumeSlider.value = AudioListener.volume;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 0;
        Data data = SystemSave.LoadPlayer();
        if (data != null)
        {
            if (data.sceneBuild != 0)
            {
                newGameButton.SetActive(true);
                startText.text = "New Game";
                currentScene = data.sceneBuild;
            }

        }

    }
    private void Update()
    {
        if (player.transform.position.x < tutorialTrigger.position.x)
        {
            tutorialPanel.SetActive(true);
        }
        else
        {
            tutorialPanel.SetActive(false);
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        player.GetComponent<PlayerData>().LoadWithOutScene();
        menuMusic.Pause();
        background.Play();
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
        canvasManager.MenuSceneBanner();
        player.GetComponent<SpriteRenderer>().enabled = true;
        anim.SetTrigger("isStart");
        StartCoroutine(StartGameAfterDelay());

    }
    IEnumerator StartGameAfterDelay()
    {
        yield return new WaitForSeconds(1);
        anim.enabled = false;
        playerController.enabled = true;
        player.GetComponent<PlayerHealth>().enabled = true;

    }
    public void LoadScene()
    {
        Time.timeScale = 1;
        menuMusic.Pause();
        StartCoroutine(LoadSceneAfterDelay());
    }
    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(.5f);
        player.GetComponent<PlayerData>().Load();
    }
    void SetLevelButtonDisabled()
    {
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
        spikeHurt.SetActive(true);
        menuMusic.Stop();
        StartCoroutine(LoadLevelAfterDelay(level));

    }
    IEnumerator LoadLevelAfterDelay(int level)
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(level);
    }
    public void SpikeSetActive()
    {
        spikeHurt.SetActive(true);
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
