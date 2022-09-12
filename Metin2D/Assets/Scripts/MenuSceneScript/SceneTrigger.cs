using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] CanvasManager canvasManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerData>().currentScene += 1;
            other.gameObject.GetComponent<PlayerData>().Save();
            NextScene();
        }
    }
    void NextScene()
    {
        Time.timeScale = 1;
        canvasManager.SpikeHurtActive();
        StartCoroutine(RestartAfterDelay());

    }
    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
