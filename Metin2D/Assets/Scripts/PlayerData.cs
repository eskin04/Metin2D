using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    public float totalCoin;
    public int currentScene;
    PlayerController playerSc;


    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        playerSc = GetComponent<PlayerController>();
        if (currentScene != 0)
            Load();

    }

    public void Save()
    {
        SystemSave.SavePlayer(this);
    }
    public void Load()
    {
        Data data = SystemSave.LoadPlayer();
        if (data != null)
        {
            currentScene = data.sceneBuild;
            totalCoin = data.coin;
            if (currentScene != SceneManager.GetActiveScene().buildIndex)
                SceneManager.LoadScene(currentScene);
        }

    }
    public void Remove()
    {
        SystemSave.RemovePlayer(this);

    }
}
