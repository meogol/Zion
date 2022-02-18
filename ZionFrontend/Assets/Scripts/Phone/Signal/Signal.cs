using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal
{
    public int speed { get; set; }
    public SignalType signalType { get; set; }
    private int c = 299792458; //скорость света в м/с
    private float distance { get; set; }

    public Signal()
    {
        speed = 0;
        signalType = SignalType.Mbps;
    }

    public string getSignal()
    {
        return speed + "\t" + signalType;
    }

    public float Power(float distance)
    {
        float AntenaPower = 1; //мощность подаваемая на антену базовой станции
        float f = (30 + distance * 2.16f) * Mathf.Pow(10, 9); //частота
        int x = 1;// UnityEngine.Random.Range(0, 2); //изменяющаяся во времени рандомизированная переменная [0..2]
        float powerOfTower = AntenaPower * Mathf.Pow((c / (4 * Mathf.PI * distance * f)), 2);//мощность вышки
        float signalPower = 10 * Mathf.Log10(powerOfTower * x) + 30;//сила сигнала
        this.signalType = SignalType.dBm;

        return signalPower;
    }

}
