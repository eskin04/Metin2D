using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    public float totalCoin;
    public int currentScene;
    public float maxFireBall;
    public float dashCoolDown;
    public float updateCoinDash;
    public float updateCoinFire;
    PlayerController playerSc;
    [SerializeField] CanvasManager canvasManager;

    // Start is called before the first frame update
    void Start()
    {
        playerSc = GetComponent<PlayerController>();
        if (SceneManager.GetActiveScene().buildIndex != 0)
            LoadWithOutScene();
    }

    public void Save()
    {
        SystemSave.SavePlayer(this);
    }
    public void Load()
    {
        LoadWithOutScene();
        Time.timeScale = 1;
        StartCoroutine(LoadAfterDelay());


    }
    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(currentScene);
    }
    public void LoadWithOutScene()
    {
        Data data = SystemSave.LoadPlayer();
        if (data != null)
        {
            currentScene = data.sceneBuild;
            totalCoin = data.coin;
            maxFireBall = data.maxFireBall;
            dashCoolDown = data.dashCoolDown;
            updateCoinDash = data.updateCoinDash;
            updateCoinFire = data.updateCoinFire;
        }


        playerSc.fireBallCount = maxFireBall;
        if (canvasManager)
        {
            canvasManager.FireBallText(maxFireBall);
            canvasManager.CoinText(totalCoin);
        }

    }
    public void Remove()
    {
        SystemSave.RemovePlayer(this);
        Save();
    }
}
