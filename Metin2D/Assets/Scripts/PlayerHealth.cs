using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    GameObject[] healthImgs;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthImgs ??= GameObject.FindGameObjectsWithTag("HealthImg");
        SetMaxHealthImg();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetMaxHealthImg()
    {
        if (healthImgs != null)
        {
            for (int i = 0; i < maxHealth; i++)
            {
                healthImgs[i].GetComponent<Image>().color = Color.white;

            }
        }

    }
    public void SetHealthImg()
    {
        if (healthImgs != null && health >= 0)
        {
            for (int i = 0; i < health; i++)
            {
                healthImgs[i].GetComponent<Image>().color = Color.white;
            }
            for (int i = health; i < maxHealth; i++)
            {
                healthImgs[i].GetComponent<Image>().color = new Color32(65, 65, 65, 255);

            }
        }
    }



}
