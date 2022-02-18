using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal
{
    public int speed { get; set; }
    public SignalType signalType { get; set; }
    
    private int c = 299792458; //�������� ����� � �/�
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
        float AntenaPower = 1; //�������� ���������� �� ������ ������� �������
        float f = (30 + distance * 2.16f) * Mathf.Pow(10, 9); //�������
        int x = 1;// UnityEngine.Random.Range(0, 2); //������������ �� ������� ����������������� ���������� [0..2]
        float powerOfTower = AntenaPower * Mathf.Pow((c / (4 * Mathf.PI * distance * f)), 2);//�������� �����
        float signalPower = 10 * Mathf.Log10(powerOfTower * x) + 30;//���� �������
        this.signalType = SignalType.dBm;

        return signalPower;
    }

}
