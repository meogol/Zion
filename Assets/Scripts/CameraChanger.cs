using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    private Camera Thecamera;
    public Camera Cam;

    void Start()
    {
        Thecamera = GetComponent<Camera>();
        Thecamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Thecamera.enabled = ! Thecamera.enabled;
            Cam.enabled = ! Cam.enabled;
        }

    }
}
