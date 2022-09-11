using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fireBallText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] GameObject spikeHurt;
    [SerializeField] GameObject nextLevelPanel;
    [SerializeField] GameObject parshmentImage;
    [SerializeField] Button dashButton;
    [SerializeField] Button fireButton;
    [SerializeField] TextMeshProUGUI oldDashText;
    [SerializeField] TextMeshProUGUI newDashText;
    [SerializeField] TextMeshProUGUI oldFireText;
    [SerializeField] TextMeshProUGUI newFireText;
    [SerializeField] TextMeshProUGUI dashUpgradeText;
    [SerializeField] TextMeshProUGUI fireUpgradeText;
    [SerializeField] TextMeshProUGUI levelBannerText;
    [SerializeField] GameObject nextLevelText;
    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioClip buttonSound;
    AudioSource audioSource;
    public bool isNextLevelPanel;
    bool isVictory;
    public bool isNextLevelText;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().name == "BossScene")
        {
            levelBannerText.text = "Boss Stage";
        }
        else if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            levelBannerText.text = "Stage " + SceneManager.GetActiveScene().buildIndex.ToString();
        }
    }
    private void Update()
    {

        if (isNextLevelPanel && !isVictory)
        {
            Data data = SystemSave.LoadPlayer();
            nextLevelPanel.SetActive(true);
            oldDashText.text = data.dashCoolDown.ToString();
            oldFireText.text = data.maxFireBall.ToString();
            newDashText.text = (data.dashCoolDown - 0.2f).ToString();
            newFireText.text = (data.maxFireBall + 2).ToString();
            dashUpgradeText.text = data.updateCoinDash.ToString();
            fireUpgradeText.text = data.updateCoinFire.ToString();
            if (data.coin < data.updateCoinDash)
            {
                dashButton.interactable = false;
            }
            if (data.coin < data.updateCoinFire)
            {
                fireButton.interactable = false;
            }
        }

    }
    public void MenuSceneBanner()
    {
        levelBannerText.text = "Tutorial";
    }

    public void FireBallText(float fireBallCount)
    {
        fireBallText.text = fireBallCount.ToString();
    }
    public void CoinText(float totalCoin)
    {
        if (coinText != null)
        {
            coinText.text = totalCoin.ToString();
        }
    }
    public void SpikeHurtActive()
    {
        spikeHurt.SetActive(true);
    }
    public void SpikeHurtInactive()
    {
        spikeHurt.SetActive(false);
    }
    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound, .5f);
    }
    public void PlayButtonSound()
    {
        audioSource.PlayOneShot(buttonSound, .2f);
    }
    public void NextLevel()
    {
        Time.timeScale = 1;
        SpikeHurtActive();
        StartCoroutine(NextLevelAfterDelay());
    }
    IEnumerator NextLevelAfterDelay()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SpikeHurtActive();
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
        SpikeHurtActive();
        StartCoroutine(MainMenuAfterDelay());
    }
    IEnumerator MainMenuAfterDelay()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(0);
    }
    public void NextLevelText()
    {
        nextLevelText.SetActive(true);
        isNextLevelText = true;
    }
    public void NextLevelTextInactive()
    {
        nextLevelText.SetActive(false);
        isNextLevelText = false;
    }
    public void SetParshmentImage()
    {
        parshmentImage.GetComponent<Image>().color = Color.white;
    }
    public void Victory()
    {
        Time.timeScale = 0;
        isVictory = true;
        isNextLevelPanel = true;
        levelBannerText.text = "Okan Eskin";
        nextLevelText.SetActive(true);
        nextLevelPanel.SetActive(true);
    }

}
