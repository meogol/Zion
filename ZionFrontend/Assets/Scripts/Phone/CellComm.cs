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
    public Signal signal { get; set; }
    public Dictionary<string, float> connections = new Dictionary<string, float>();
    private int c = 299792458; //скорость света в м/с

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
                
                float AntenaPower = 10000; //мощность подаваемая на антену базовой станции
                float f = (30+distance*2.16f)*Mathf.Pow(10, 9); //частота
                int x = 1;//UnityEngine.Random.Range(0, 2); //изменяющаяся во времени рандомизированная переменная [0..2]
                float powerOfTower = AntenaPower * Mathf.Pow((c / (4 * Mathf.PI * distance * f)), 2);//мощность вышки
                float signalPower = 10 * Mathf.Log10(powerOfTower * x) + 30;//сила сигнала        
                signal = new Signal(Convert.ToInt32(signalPower));
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        
    }

}

