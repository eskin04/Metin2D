using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    public int sceneBuild;
    public float coin;

    public Data(PlayerData player)
    {
        sceneBuild = player.currentScene;
        coin = player.totalCoin;
    }

}
