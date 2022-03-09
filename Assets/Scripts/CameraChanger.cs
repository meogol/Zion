using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    private Camera Thecamera;
    public Camera Cam;

    public GameObject exitMenu;

    private bool clicker;
    private bool cursorVisibility;

    public GameObject player;

    void OffUserControllScripts()
    {
        player.GetComponent<PhoneChanger>().enabled = !player.GetComponent<PhoneChanger>().isActiveAndEnabled;
        
    }
    void Start()
    {
        Thecamera = GetComponent<Camera>();
        Thecamera = Camera.main;

        cursorVisibility = false;
        clicker = false;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffUserControllScripts();

            Thecamera.enabled = ! Thecamera.enabled;
            Cam.enabled = ! Cam.enabled;

            exitMenu.SetActive(!clicker);


            Cursor.lockState = CursorLockMode.None;
            
            if (cursorVisibility == false)
            {
                Cursor.visible = !cursorVisibility;
            }

            if (!Cam.isActiveAndEnabled)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        
    }
}
