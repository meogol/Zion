using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Ping = System.Net.NetworkInformation.Ping;
using System;

public class PingRequests : MonoBehaviour
{
    public string noConnection = "No connection !!!";
    public string CallPing()
    {   

        Ping pingSender = new Ping();
        PingOptions pingOptions = new PingOptions();


        pingOptions.DontFragment = true;


        string data = "Testing request. Hello, fellow comrads!";
        byte[] buffer = Encoding.ASCII.GetBytes(data);
        int timeout = 120;

        PingReply reply = pingSender.Send("www.google.com", timeout, buffer, pingOptions);
        if (reply.Status == IPStatus.Success)
        {
            /// Debug.Log(reply.Address);
            ///Debug.Log(reply.Options.Ttl);
            ///Debug.Log(reply.Options.DontFragment);
            ///Debug.Log(reply.Buffer);
            return reply.RoundtripTime.ToString();
        }
        else
        {
            
            return noConnection;
        }

    }
}
