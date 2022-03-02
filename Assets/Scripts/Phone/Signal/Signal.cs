using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal
{
    public int speed { get; set; }
    public int power { get; set; }
    public int collisionsCount { get; set; }
    public SignalType signalType { get; set; }

    private int c = 299792458; //скорость света в м/с
    public float distance { get; set; }

    public Signal()
    {
        speed = 0;
        signalType = SignalType.Mbps;
    }

    public string getSignal()
    {
        return speed + "\t" + signalType;
    }

    public int Power(float distance, int collision, float radius, int countOfUsers)
    {
        this.distance = distance;
        int AntenaPower = 40000; //мощность подаваемая на антену базовой станции
        float minHz = 30;
        float maxHz = 300;
        float coefficient = - (minHz - maxHz) / radius;
        float f = (30 + distance * coefficient) * Mathf.Pow(10, 9); //частота
        int x = 2; // TODO: изменяющаяся во времени рандомизированная переменная (0..2], возможно придется сделать через Random()
        float powerOfTower = AntenaPower * Mathf.Pow((c / (4 * Mathf.PI * distance * f)), 2);//мощность вышки

        float A = 1, B = 0;//коэффициенты для потерь, где А=5 и В=0.03 для низкой задержки,
                           //а А=10 и В=5 для высокой

        if ((collision >= 1) & (collision < 2))
        {
            A = 5;
            B = 0.03f;
        }
        else if (collision >= 2)
        {
            A = 10;
            B = 5;
        }
        float Bpl = 10 * Mathf.Log10(A + B * Mathf.Pow((f / Mathf.Pow(10, 9)), 2));//потеря сигнала в зависимости от количества колизий

        float signalPower = 15 * Mathf.Log10( powerOfTower * x) + 30 - Bpl;//сила сигнала



        if(countOfUsers > 256)
        {
            this.power -= System.Convert.ToInt32(0.2 * countOfUsers);
        }

        return System.Convert.ToInt32(signalPower);
    }

    public string GetNetIndexator()
    {
        string netIndexator = "";

        if(power >= -120)
        {
            netIndexator += "<color=green>.</color>";
            if(power >= -100)
            {
                netIndexator += "<color=green>п</color>";
                if(power >= -70)
                {
                    netIndexator += "<color=green>П</color>";
                }
                else
                {
                    netIndexator += "<color=grey>П</color>";
                }
            }
            else
            {
                netIndexator += "<color=grey>пП</color>";
            }
        }
        else
        {
            netIndexator += "<color=red>no signal...</color>";
        }

        return netIndexator;
    }


}
