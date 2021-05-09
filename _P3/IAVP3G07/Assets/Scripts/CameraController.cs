using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera countCamera;
    [SerializeField] Camera singerCamera;
    [SerializeField] Camera ghostCamera;
    int activeCam = 0;
    // Update is called once per frame
    void Update()
    {
        if(activeCam != 0 && Input.GetKey(KeyCode.V))
        {
            countCamera.enabled = true;
            singerCamera.enabled = false;
            ghostCamera.enabled = false;
            activeCam = 0;
        }
        else if (activeCam != 1 && Input.GetKey(KeyCode.C))
        {
            countCamera.enabled = false;
            singerCamera.enabled = true;
            ghostCamera.enabled = false;
            activeCam = 1;
        }
        else if (activeCam != 2 && Input.GetKey(KeyCode.F))
        {
            countCamera.enabled = false;
            singerCamera.enabled = false;
            ghostCamera.enabled = true;
            activeCam = 2;
        }
    }
}
