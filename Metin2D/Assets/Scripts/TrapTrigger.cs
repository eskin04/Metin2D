using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField] GameObject spearPref;
    [SerializeField] float spawnX;
    [SerializeField] float spawnY;
    bool isEnd;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag=="Player" && !isEnd)
        {
            Vector2 spearPos = new Vector2(spawnX,spawnY);
            Vector2 triggerPos = new Vector2(transform.position.x,transform.position.y);
            isEnd=true;
            Instantiate(spearPref,spearPos+triggerPos,spearPref.transform.rotation);
        }
    }
}
