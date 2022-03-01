using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Signal
{
    public int speed { get; set; }
    public int power { get; set; }
    public int collisionsCount { get; set; }
    public SignalType signalType { get; set; }

    private int c = 299792458; //�������� ����� � �/�
    public float distance { get; set; }



    public ThrottledStream stream { get; set; }

    //private const int MAX_BPS = 1342177280;

    Stream parentStream;



    public Signal()
    {
        speed = 0;
        signalType = SignalType.Mbps;

        // �������� ������ � ���������� ����������� ��������
        // parentStream - ����� ������
        stream = new ThrottledStream(parentStream, speed);

    }

    public string getSignal()
    {
        return speed + "\t" + signalType;
    }

    public int Power(float distance, int collision, float radius, int countOfUsers)
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
            this.power -= System.Convert.ToInt32(0.2 * countOfUsers);
        }


        // ������ ������� �������� �������!!!

        ChangeMaxBPS(speed);


    }

    public void ChangeMaxBPS(int bps)
    {
        stream.MaxBytesPerSecond = bps;
    }


    public string GetNetIndexator()
    {
        string netIndexator = "";

        if(power >= -120)
        {
            netIndexator += "<color=green>.</color>";
            if(power >= -100)
            {
                netIndexator += "<color=green>�</color>";
                if(power >= -70)
                {
                    netIndexator += "<color=green>�</color>";
                }
                else
                {
                    netIndexator += "<color=grey>�</color>";
                }
            }
            else
            {
                netIndexator += "<color=grey>��</color>";
            }
        }
        else
        {
            netIndexator += "<color=red>no signal...</color>";
        }

        return netIndexator;
    }


}
