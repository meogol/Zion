using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTab : MonoBehaviour
{
    private Camera Thecamera;
    public Camera Cam;

    // Start is called before the first frame update
    void Start()
    {
        Thecamera = GetComponent<Camera>();
        Thecamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Closetab()
    {
        Thecamera.enabled = !Thecamera.enabled;
        Cam.enabled = !Cam.enabled;
    }
}
