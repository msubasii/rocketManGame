using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera fixedCamera;
    public CinemachineVirtualCamera followCamera;

    public bool transitionStarted = false;

    void Start()
    {
        fixedCamera.Priority = 11;
        followCamera.Priority = 10;
    }

    private void FixedUpdate()
    {
        if (transitionStarted)
        {
            CameraSwitch();
        }
    }
    public void CameraSwitch()
    {
        fixedCamera.Priority = 9;
        followCamera.Priority = 12;
    }
}
