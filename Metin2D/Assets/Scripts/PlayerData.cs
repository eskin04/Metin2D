using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    PlayerController playerSc;
    // Start is called before the first frame update
    void Start()
    {
        playerSc = GetComponent<PlayerController>();
        playerSc.health = 7;
    }

    public void Save()
    {
        SystemSave.SavePlayer(playerSc);
    }
    public void Load()
    {
        Data data = SystemSave.LoadPlayer();
        if (data != null)
        {
            playerSc.currentScene = data.sceneBuild;
            playerSc.health = data.health;
            playerSc.totalCoin = data.coin;
            SceneManager.LoadScene(playerSc.currentScene);
            playerSc.UpdateHealth(0);
        }
    }
}
