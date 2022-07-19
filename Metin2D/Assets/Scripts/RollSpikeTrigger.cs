using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollSpikeTrigger : MonoBehaviour
{
    [SerializeField] GameObject spikePref;
    [SerializeField] float spikeCount;
    [SerializeField] float coolDown;
    [SerializeField] Vector2 spikePos;
    bool isFirst;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player" && !isFirst) {
            isFirst=true;
            StartCoroutine(SpawnSpikes());

            
        }
    }
    IEnumerator SpawnSpikes() {
        for (int i = 0; i < spikeCount; i++) {
            GameObject spike = Instantiate(spikePref, spikePos, spikePref.transform.rotation);
            yield return new WaitForSeconds(coolDown);
        }
        Destroy(gameObject);
        
    }
}
