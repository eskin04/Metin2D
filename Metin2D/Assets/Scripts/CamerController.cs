using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smoothing;
    [SerializeField] Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetCamPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetCamPos + offset, smoothing * Time.deltaTime);
    }
}
