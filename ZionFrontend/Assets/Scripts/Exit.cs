using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private bool isClicked;

    // Start is called before the first frame update
    void Start()
    {
        isClicked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isClicked == true)
        {
            Application.Quit();
        }
    }
}
