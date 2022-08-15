using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fireBallText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] GameObject spikeHurt;


    public void FireBallText(float fireBallCount)
    {
        fireBallText.text = fireBallCount.ToString();
    }
    public void CoinText(float totalCoin)
    {
        coinText.text = totalCoin.ToString();
    }
    public void SpikeHurtActive()
    {
        spikeHurt.SetActive(true);
    }
    public void SpikeHurtInactive()
    {
        spikeHurt.SetActive(false);
    }
}
