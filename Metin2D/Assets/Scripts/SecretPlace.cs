using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretPlace : MonoBehaviour
{
    float live;
    [SerializeField] GameObject secretPlace;
    // Start is called before the first frame update
    void Start()
    {
        live =2;
    }

    // Update is called once per frame
    void Update()
    {
        if(live <=0)
        {
            secretPlace.SetActive(true);
            Destroy(gameObject);
        }
    }
    public void DestroyPlace(float damage)
    {
        live +=damage;
    }
}
