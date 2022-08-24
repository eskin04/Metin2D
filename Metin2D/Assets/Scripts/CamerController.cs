using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smoothing;
    [SerializeField] Vector3 offset;
    [SerializeField] PlayerController playerSc;
    bool isPressS;
    bool isKeyUpS;
    Vector3 lookDown;
    // Start is called before the first frame update
    void Start()
    {
        lookDown = offset;
    }

    // Update is called once per frame
    void Update()
    {


        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (!playerSc.isKnockBack)
        {
            CameraMove();
        }

        if ((vertical != 0) && !playerSc.isInLadder && !isPressS)
        {
            isPressS = true;
            isKeyUpS = false;
            if (vertical < 0)
            {
                StartCoroutine(LookDown());

            }
            else
            {
                StartCoroutine(LookUp());
            }
        }
        if (!isKeyUpS && (horizontal != 0 || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.W) || Input.GetMouseButtonDown(0)))
        {
            StopAllCoroutines();
            offset = lookDown;
            isKeyUpS = true;
            StartCoroutine(WaitPressS());
        }

    }
    void CameraMove()
    {
        Vector3 targetCamPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetCamPos + offset, smoothing * Time.deltaTime);
    }
    IEnumerator WaitPressS()
    {
        yield return new WaitForSeconds(.5f);
        isPressS = false;
    }
    IEnumerator LookDown()
    {
        yield return new WaitForSeconds(.5f);
        offset = new Vector3(offset.x, offset.y - 4f, offset.z);
    }
    IEnumerator LookUp()
    {
        yield return new WaitForSeconds(.5f);
        offset = new Vector3(offset.x, offset.y + 3f, offset.z);
    }
}
