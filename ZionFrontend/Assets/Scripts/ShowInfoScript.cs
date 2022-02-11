using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ShowInfoScript : MonoBehaviour
{
    private String input;
    private TextMeshPro text;
    

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        input = "SAMSUKA 10E";
    }

    // Update is called once per frame
    void Update()
    {
        text.text = input;
    }
}
