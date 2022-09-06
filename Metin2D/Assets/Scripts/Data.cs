using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    public int sceneBuild;
    public float coin;
    public float maxFireBall;
    public float dashCoolDown;
    public float updateCoinDash;
    public float updateCoinFire;

    public Data(PlayerData player)
    {
        sceneBuild = player.currentScene;
        coin = player.totalCoin;
        maxFireBall = player.maxFireBall;
        dashCoolDown = player.dashCoolDown;
        updateCoinDash = player.updateCoinDash;
        updateCoinFire = player.updateCoinFire;
    }

}
