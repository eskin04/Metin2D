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
    float lookDownY;
    float firstOffsetX;
    bool isKeyUpHorizontal;
    bool isPressHorizontal;
    // Start is called before the first frame update
    void Start()
    {
        lookDownY = offset.y;
        firstOffsetX = offset.x;
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
                StartCoroutine("LookDown");

            }
            else
            {
                StartCoroutine("LookUp");
            }
        }
        if (!isKeyUpS && (horizontal != 0 || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.W) || Input.GetMouseButtonDown(0)))
        {
            StopCoroutine("LookDown");
            StopCoroutine("LookUp");
            offset = new Vector3(offset.x, lookDownY, offset.z);
            isKeyUpS = true;
            StartCoroutine(WaitPressS());
        }
        if (horizontal != 0 && !playerSc.isInLadder && !isPressHorizontal)
        {
            isPressHorizontal = true;
            isKeyUpHorizontal = false;
            if (horizontal < 0)
            {
                StartCoroutine("CoolDownBig");
            }
            else
            {
                StartCoroutine("CoolDownSmall");
            }
        }
        if (!isKeyUpHorizontal && (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)))
        {
            isKeyUpHorizontal = true;
            StopCoroutine("CoolDownBig");
            StopCoroutine("CoolDownSmall");
            StartCoroutine(WaitPressHorizontal());

        }

        // if (horizontal < 0 && !coolDown)
        // {
        //     StopCoroutine(CoolDownSmall());
        //     coolDown = true;
        //     StartCoroutine(CoolDownBig());
        // }
        // else if (horizontal > 0 && coolDown)
        // {
        //     coolDown = false;
        //     StopCoroutine(CoolDownBig());
        //     StartCoroutine(CoolDownSmall());
        // }

    }
    IEnumerator WaitPressHorizontal()
    {
        yield return new WaitForSeconds(0.5f);
        isPressHorizontal = false;
    }
    IEnumerator CoolDownBig()
    {
        yield return new WaitForSeconds(.5f);
        offset = new Vector3(-firstOffsetX, offset.y, offset.z);

    }
    IEnumerator CoolDownSmall()
    {
        yield return new WaitForSeconds(.5f);
        offset = new Vector3(firstOffsetX, offset.y, offset.z);
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
