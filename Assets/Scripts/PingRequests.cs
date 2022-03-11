using UnityEngine;
using System.Net.NetworkInformation;
using System.Text;
using Ping = System.Net.NetworkInformation.Ping;


public class PingRequests : MonoBehaviour
{
    public string noConnection = "No connection !!!";

    public string ping;

    private Ping pingSender = new Ping();
    private PingOptions pingOptions = new PingOptions();
    private static string data = "Testing request. Hello, fellow comrads!";
    byte[] buffer = Encoding.ASCII.GetBytes(data);
    static int timeout = 120;
    private PingReply reply;

    public void CallPing()
    {   
        pingOptions.DontFragment = true;

        while (true)
        {
            reply = pingSender.Send("yandex.ru", timeout, buffer, pingOptions);
            if (reply.Status == IPStatus.Success)
            {
                Debug.Log(reply.Address);

                ping = reply.RoundtripTime.ToString();
            }
            else
            {

                ping = noConnection;
            }
        }
    }
}
