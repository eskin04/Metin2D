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


    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        playerSc = GetComponent<PlayerController>();
        if (currentScene != 0)
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

    }
    public void Remove()
    {
        SystemSave.RemovePlayer(this);

    }
}
