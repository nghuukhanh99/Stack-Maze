using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public static MoveCamera Instance;

    public GameObject Camera;

    public Transform currentCamPos;

    public Vector3 nextCamPos1;

    public Vector3 nextCamPos2;

    public Vector3 nextCamPos3;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void LateUpdate()
    {
        if(PlayerCtrlTest.Instance.CamMovePos1 == true)
        {
            MoveCamPos1();
        }

        if (PlayerCtrlTest.Instance.CamMovePos2 == true)
        {
            MoveCamPos2();
        }

        if (PlayerCtrlTest.Instance.CamMovePos3 == true)
        {
            MoveCamPos3();
        }


    }
    public void MoveCamPos1()
    {
        Camera.transform.position = Vector3.MoveTowards(currentCamPos.transform.position, nextCamPos1, 20f * Time.smoothDeltaTime);
    }

    public void MoveCamPos2()
    {
        Camera.transform.position = Vector3.MoveTowards(currentCamPos.transform.position, nextCamPos2, 20f * Time.smoothDeltaTime);
    }

    public void MoveCamPos3()
    {
        Camera.transform.position = Vector3.MoveTowards(currentCamPos.transform.position, nextCamPos3, 20f * Time.smoothDeltaTime);

        Camera.transform.rotation = Quaternion.Euler(new Vector3(50f, 0f, 0f));
    }
}
