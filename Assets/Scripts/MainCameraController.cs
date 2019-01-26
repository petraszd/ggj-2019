using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera m_virtualCamera;

    // Start is called before the first frame update
    void Start()
    {

        if (m_virtualCamera.Follow == null) {
            m_virtualCamera.Follow = GameObject.Find("Girl").GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
