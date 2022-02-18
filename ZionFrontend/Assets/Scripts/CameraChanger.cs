using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    private Camera TheCamera;
    public Camera Cam;

    // Start is called before the first frame update
    void Start()
    {
        TheCamera = GetComponent<Camera>();
        TheCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TheCamera.enabled = !TheCamera;
            Cam.enabled = !Cam.enabled;
        }

    }
}
