using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal
{
    public int speed { get; set; }
    public SignalType signalType { get; set; }
   
    public Signal()
    {
        speed = 0;
        signalType = SignalType.Mbps;
    }

    public Signal(int speed, SignalType signalType = SignalType.dBm)
    {
        this.speed = speed;
        this.signalType = signalType;
    }

    public string getSignal()
    {
        return speed + "\t" + signalType;
    }



}
