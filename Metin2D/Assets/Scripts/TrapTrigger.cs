using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField] GameObject spearPref;
    [SerializeField] Vector2 spawnPos;
    bool isEnd;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag=="Player" && !isEnd)
        {
            isEnd=true;
            Instantiate(spearPref,new Vector2(spawnPos.x,spawnPos.y),spearPref.transform.rotation);
        }
    }
}
