using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField] GameObject spearPref;
    [SerializeField] float spearCount;
    [SerializeField] float coolDown;
    [SerializeField] float spearRange;
    [SerializeField] float spawnX;
    [SerializeField] float spawnY;
    bool isEnd;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag=="Player" && !isEnd)
        {
            isEnd=true;
            StartCoroutine(spearTimer());
        }
    }
    IEnumerator spearTimer()
    {
        Vector2 spearPos = new Vector2(spawnX,spawnY);
        float firstRange=spearRange;
        for (int i = 0; i < spearCount; i++)
        {
            Vector2 triggerPos = new Vector2(transform.position.x+spearRange,transform.position.y);
            Instantiate(spearPref,spearPos+triggerPos,spearPref.transform.rotation);
            yield return new WaitForSeconds(coolDown);
            spearRange+=firstRange;
        }
    }

}
