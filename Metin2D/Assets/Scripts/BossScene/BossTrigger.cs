using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] GameObject bossScene;
    [SerializeField] CamerController camContrl;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "CameraObj")
        {
            camContrl.enabled = false;
            Camera.main.transform.position = new Vector3(-1.9f, 7.18f, -10);
            bossScene.SetActive(true);
            Destroy(gameObject);
        }

    }
}
