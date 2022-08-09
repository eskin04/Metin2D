using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    public int sceneBuild;
    public float coin;
    public int health;

    public Data(PlayerController player)
    {
        sceneBuild = player.currentScene;
        health = player.health;
        coin = player.totalCoin;
    }

}
