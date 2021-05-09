using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera countCamera;
    [SerializeField] Camera singerCamera;
    [SerializeField] Camera ghostCamera;
    int activeCam = 3;
    // Update is called once per frame
    void Update()
    {
        if(activeCam != 0 && Input.GetKey(KeyCode.V))
        {
            mainCamera.enabled = false;
            countCamera.enabled = true;
            singerCamera.enabled = false;
            ghostCamera.enabled = false;
            activeCam = 0;
        }
        else if (activeCam != 1 && Input.GetKey(KeyCode.C))
        {
            mainCamera.enabled = false;
            countCamera.enabled = false;
            singerCamera.enabled = true;
            ghostCamera.enabled = false;
            activeCam = 1;
        }
        else if (activeCam != 2 && Input.GetKey(KeyCode.F))
        {
            mainCamera.enabled = false;
            countCamera.enabled = false;
            singerCamera.enabled = false;
            ghostCamera.enabled = true;
            activeCam = 2;
        }
        else if (activeCam != 3 && Input.GetKey(KeyCode.G))
        {
            mainCamera.enabled = true;
            countCamera.enabled = false;
            singerCamera.enabled = false;
            ghostCamera.enabled = false;
            activeCam = 3;
        }
    }
}
