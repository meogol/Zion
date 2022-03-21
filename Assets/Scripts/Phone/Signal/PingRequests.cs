using UnityEngine;
using System.Net.NetworkInformation;
using System.Text;
using Ping = System.Net.NetworkInformation.Ping;
using System.Diagnostics;

public class PingRequests : MonoBehaviour
{
    public string noConnection = "No connection !!!";
    public string ping;
    public int inputCount;
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
        int InputCount = 0;

        while (true)
        {
            stopwatch.Start();
            reply = pingSender.Send("yandex.ru", timeout, buffer, pingOptions);
            if (reply.Status == IPStatus.Success)
            {
                //UnityEngine.Debug.Log(reply.Address);

                ping = reply.RoundtripTime.ToString();
            }
            else
            {

                ping = noConnection;
            }
            stopwatch.Stop();
            InputCount++;
            
            if (stopwatch.ElapsedMilliseconds >= 1000)
            {
                inputCount = InputCount;
                InputCount = 0;
                stopwatch.Reset();
            }
            
        }
    }
}
