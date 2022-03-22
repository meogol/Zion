using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System;

public class Signal
{
    private int speedMBPS { get; set; }
    public int speed { get; set; }
    public int power { get; set; }
    public SignalType signalType { get; set; }

    private int c = 299792458;
    private float[] CountTime = new float[10] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
    private float[] CountServ = new float[10] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    private float distance { get; set; }
    private float sum;
    private double SumDev;
    private float AverValue;
    public float SKO;
    private int DontTouchTime = 0;
    private int DontTouchTimeServ = 0;
    private int Nb = 2;//TODO:buffer size in MB
    private float step;//TODO:power - law construction
    public float PL;//TODO: Packet Loss
    public float p;//TODO:loading the system n/256, n - count users

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

    public int PacketLoss(int inputCount, int countOfUsers = 1)
    {
        p = (float)(countOfUsers / 256.0);
        step = (float)(2 / (System.Math.Pow((float)(SKOTime(inputCount) / 10.0), 2) + System.Math.Pow((float)(SKOServ(inputCount) / 10.0), 2)) * Nb);
        PL = ((float)((1 - p) / (1 - System.Math.Pow(p, step + 1)) * System.Math.Pow(p, step)));
        return System.Convert.ToInt32(PL*100);
    }

    private float SKOTime(int inputCount)
    {
        CountTime[DontTouchTime++] = inputCount; 
        sum=0;
        for (int i=0; i < 10; i++) 
        { 
            sum += CountTime[i]; 
        }
        AverValue = (float)(sum / 10.0);
        SumDev = 0;
        for (int i=0; i < 10; i++) 
        { 
            SumDev += System.Math.Pow((CountTime[i] - AverValue), 2); 
        }
        SKO = (float)System.Math.Sqrt(SumDev/10.0);
        if (DontTouchTime == 9) { DontTouchTime = 0; }
        return SKO;
    }

    private float SKOServ(int inputCount)
    {
        CountServ[DontTouchTimeServ++] = Mathf.Round(1000 / inputCount)/100;
        sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += CountServ[i];
        }
        AverValue = (float)(sum / 10.0);
        SumDev = 0;
        for (int i = 0; i < 10; i++) 
        { 
            SumDev += System.Math.Pow((CountServ[i] - AverValue), 2); 
        }
        SKO = (float)System.Math.Sqrt(SumDev / 10.0);
        if (DontTouchTimeServ == 9) { DontTouchTimeServ = 0; }
        return SKO;
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
