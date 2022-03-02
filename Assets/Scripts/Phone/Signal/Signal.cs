using System.IO;
using UnityEngine;


public class Signal
{
    public int speed { get; set; }
    public int power { get; set; }
    public int collisionsCount { get; set; }
    public SignalType signalType { get; set; }

    private int c = 299792458; //скорость света в м/с
    private float distance { get; set; }



    public ThrottledStream stream { get; set; }

    //private const int MAX_BPS = 1342177280;




    public Signal()
    {
        speed = 0;
        signalType = SignalType.Mbps;

        //Some DataStream creating
        //Stream dataStream = new Stream();
        //stream = new ThrottledStream(dataStream, speed);

    }

    public string getSignal()
    {
        return speed + "\t" + signalType;
    }

    public void ChangeSignal(float distance, int collision, float radius, int countOfUsers)
    {
        ChangePower(distance, collision, radius, countOfUsers);
        ChangeSpeed();
    }
    public void ChangeSignal(int power)
    {
        this.power = power;
        ChangeSpeed();
    }

    public void ChangePower(float distance, int collision, float radius, int countOfUsers)
    {
        this.distance = distance;
        int AntenaPower = 40000; //мощность подаваемая на антену базовой станции
        float minHz = 30;
        float maxHz = 300;
        float coefficient = - (minHz - maxHz) / radius;
        float f = (30 + distance * coefficient) * Mathf.Pow(10, 9); //частота
        int x = 2;// UnityEngine.Random.Range(0, 2); //изменяющаяся во времени рандомизированная переменная [0..2]
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

        this.power = System.Convert.ToInt32(signalPower);


        if(countOfUsers > 256)
        {
            this.power -= System.Convert.ToInt32(0.2 * countOfUsers);
        }

    }


    // imaginary formula fro dependency of speed from signal power
    public void ChangeSpeed()
    {
        int mbps = 0;

        if (power >= -55)
        {
            mbps = 20000;
        }
        else if (power >= -70)
        {
            mbps = 10000 + System.Convert.ToInt32((power + 70) / 0.0015);
        }
        else if (power >= -85)
        {
            mbps = 1000 + System.Convert.ToInt32((power + 85) / 0.002);
        }
        else if (power >= -100)
        { 
            mbps = 50 + System.Convert.ToInt32((power + 100) / 0.015);
        }

        if(mbps >= 1000)
        {
            speed = mbps / 1000;
            signalType = SignalType.Gbps;
        }
        else
        {
            speed = mbps;
            signalType = SignalType.Mbps;
        }


        //stream.MaxBytesPerSecond = mbps * 125000;

    }

    public string GetNetIndexator()
    {
        string netIndexator = "";

        if(power >= -100)
        {
            netIndexator += "<color=green>.</color>";
            if(power >= -85)
            {
                netIndexator += "<color=green>п</color>";
                if(power >= -55)
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
