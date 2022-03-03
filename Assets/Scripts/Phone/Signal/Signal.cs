using System.IO;
using UnityEngine;


public class Signal
{
    private int speedMBPS { get; set; }
    public int speed { get; set; }
    public int power { get; set; }
    public int collisionsCount { get; set; }
    public SignalType signalType { get; set; }

    private int c = 299792458; //�������� ����� � �/�
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
        this.power = ChangePower(distance, collision, radius, countOfUsers);
        Signal changedSignal = ChangeSpeed(power);
        this.speedMBPS = changedSignal.speedMBPS;
        this.speed = changedSignal.speed;
        this.signalType = changedSignal.signalType;

        //stream.MaxBytesPerSecond = speedMBPS * 125000;

    }
    public void ChangeSignal(int power)
    {
        this.power = power;
        Signal changedSignal = ChangeSpeed(power);
        this.speedMBPS = changedSignal.speedMBPS;
        this.speed = changedSignal.speed;
        this.signalType = changedSignal.signalType;

        //stream.MaxBytesPerSecond = speedMBPS * 125000;
    }

    public int ChangePower(float distance, int collision, float radius, int countOfUsers)
    {
        this.distance = distance;
        int AntenaPower = 40000; //�������� ���������� �� ������ ������� �������
        float minHz = 30;
        float maxHz = 300;
        float coefficient = - (minHz - maxHz) / radius;
        float f = (30 + distance * coefficient) * Mathf.Pow(10, 9); //�������
        int x = 2; // TODO: ������������ �� ������� ����������������� ���������� (0..2], �������� �������� ������� ����� Random()
        float powerOfTower = AntenaPower * Mathf.Pow((c / (4 * Mathf.PI * distance * f)), 2);//�������� �����

        float A = 1, B = 0;//������������ ��� ������, ��� �=5 � �=0.03 ��� ������ ��������,
                           //� �=10 � �=5 ��� �������

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
        float Bpl = 10 * Mathf.Log10(A + B * Mathf.Pow((f / Mathf.Pow(10, 9)), 2));//������ ������� � ����������� �� ���������� �������

        float signalPower = 15 * Mathf.Log10( powerOfTower * x) + 30 - Bpl;//���� �������



        if(countOfUsers > 256)
        {
            signalPower -= System.Convert.ToInt32(0.2 * countOfUsers);
        }

        return System.Convert.ToInt32(signalPower);

    }


    // imaginary formula fro dependency of speed from signal power
    public Signal ChangeSpeed(int signalPower)
    {
        Signal signal = new Signal();

        if (signalPower >= -55)
        {
            signal.speedMBPS = 20000;
        }
        else if (signalPower >= -70)
        {
            signal.speedMBPS = 10000 + System.Convert.ToInt32((signalPower + 70) / 0.0015);
        }
        else if (signalPower >= -85)
        {
            signal.speedMBPS = 1000 + System.Convert.ToInt32((signalPower + 85) / 0.002);
        }
        else if (signalPower >= -100)
        {
            signal.speedMBPS = 50 + System.Convert.ToInt32((signalPower + 100) / 0.015);
        }


        if (signal.speedMBPS >= 1000)
        {
            signal.speed = signal.speedMBPS / 1000;
            signal.signalType = SignalType.Gbps;
        }
        else
        {
            signal.speed = signal.speedMBPS;
            signal.signalType = SignalType.Mbps;
        }

        return signal;
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
