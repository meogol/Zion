using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CellComm : MonoBehaviour
{
    public const String DEFAULT_TEXT = "Zion 0.0\n\n      No\nconnection\n       ...";
    public GameObject obj;
    public GameObject tower;
    public String device { get; set; }
    public String output { get; set; }
    public TextMeshPro text { get; set; }
    public int power { get; set; }
    public Signal signal { get; set; }
    public Dictionary<string, float> connections = new Dictionary<string, float>();

    // Start is called before the first frame update
    void Start()
    {
        output = "";
        text = GetComponent<TextMeshPro>();
        device = "Zion 0.0";
        signal = new Signal();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            foreach (var key in connections.Keys)
            {
                tower = GameObject.FindGameObjectWithTag(key);
                Vector3 vectorDistance = tower.transform.position - transform.position;
                float distance = Mathf.Sqrt(vectorDistance.x * vectorDistance.x +
                    vectorDistance.y * vectorDistance.y + vectorDistance.z * vectorDistance.z);
                connections[key] = distance;
                

                this.power = System.Convert.ToInt32(signal.Power(distance));
            }
        }
        catch (Exception e)
        {
            //Debug.Log(e.Message);
        }
        
    }

}

