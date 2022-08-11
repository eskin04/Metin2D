using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smoothing;
    [SerializeField] Vector3 offset;
    PlayerController playerSc;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Player") != null)
        {
            playerSc ??= GameObject.Find("Player").GetComponent<PlayerController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSc == null)
        {
            Vector3 targetCamPos = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetCamPos + offset, smoothing * Time.deltaTime);
        }
        else
        {
            if (!playerSc.isKnockBack)
            {
                Vector3 targetCamPos = new Vector3(target.position.x, target.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetCamPos + offset, smoothing * Time.deltaTime);
            }
        }

    }
}
