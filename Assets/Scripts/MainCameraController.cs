using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainCameraController : MonoBehaviour
{
    //public CinemachineVirtualCamera m_virtualCamera;
    public Rect CameraLimits;
    private Transform Girl;

    // Start is called before the first frame update
    void Start()
    {
        /*
        if (m_virtualCamera.Follow == null) {
            m_virtualCamera.Follow = GameObject.Find("Girl").GetComponent<Transform>();
        }*/
        Girl = GameObject.Find("Girl").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float X, Y;

        if (Girl.position.x > CameraLimits.xMax)
        {
            X = CameraLimits.xMax;
        }
        else if (Girl.position.x < CameraLimits.xMin)
        {
            X = CameraLimits.xMin;
        }
        else
        {
            X = Girl.transform.position.x;
        }
        if (Girl.position.y > CameraLimits.yMax)
        {
            Y = CameraLimits.yMax;
        }
        else if (Girl.position.y < CameraLimits.yMin)
        {
            Y = CameraLimits.yMin;
        }
        else
        {
            Y = Girl.transform.position.y;
        }
        /*
        X = Mathf.Max(CameraLimits.xMin, Girl.position.x);
        X = Mathf.Min(CameraLimits.xMax, Girl.position.x);
        Y = Mathf.Max(CameraLimits.yMin, Girl.position.y);
        Y = Mathf.Min(CameraLimits.yMax, Girl.position.y);
        */
        transform.position = new Vector3(X, Y, 0);
    }
}
