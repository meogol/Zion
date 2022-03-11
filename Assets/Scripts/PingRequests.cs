using UnityEngine;
using System.Net.NetworkInformation;
using System.Text;
using Ping = System.Net.NetworkInformation.Ping;
using System.Diagnostics;


public class PingRequests : MonoBehaviour
{
    public string noConnection = "No connection !!!";

    public string ping;
    public int TimeMl;//TODO:package processing time

    private Ping pingSender = new Ping();
    private PingOptions pingOptions = new PingOptions();
    private static string data = "Testing request. Hello, fellow comrads!";
    byte[] buffer = Encoding.ASCII.GetBytes(data);
    static int timeout = 120;
    private PingReply reply;
    Stopwatch stopwatch = new Stopwatch();

    public void CallPing()
    {   
        pingOptions.DontFragment = true;

        while (true)
        {
            stopwatch.Start();
            reply = pingSender.Send("yandex.ru", timeout, buffer, pingOptions);
            if (reply.Status == IPStatus.Success)
            {
                UnityEngine.Debug.Log(reply.Address);

                ping = reply.RoundtripTime.ToString();
            }
            else
            {

                ping = noConnection;
            }
            stopwatch.Stop();
            int TimeMl = System.Convert.ToInt32(stopwatch.ElapsedMilliseconds);
            UnityEngine.Debug.Log("вывод твоего дерьма" + i);
            stopwatch.Reset();
        }
    }
}
