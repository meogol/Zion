using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Threading;

public class CellComm : MonoBehaviour
{
    public const String DEFAULT_TEXT = "<color=red>no signal...</color>\n0 Mbps\n_____________\nZion 0.0\n\nNo connection...";
    public GameObject obj;
    public GameObject tower;
    public String device { get; set; }
    public String output { get; set; }
    public TextMeshPro text { get; set; }
    public Dictionary<string, float> connections = new Dictionary<string, float>();

    public Dictionary<string, float> signalLossCount = new Dictionary<string, float>();
    public GameObject sphereObj;
    public CellSphere sphere;
    public Signal signal { get; set; }
    [SerializeField]
    private LayerMask mask;

    private PingRequests pingCaller = new PingRequests();

    public string bufferPing;

    private Thread pingThread;

    public RaycastHit[] lastHits = new RaycastHit[3];  // 0 - Office, 1 - Corridor, 2 - Industrial

    public int[] collisions = new int[3]; // 0 - Office, 1 - Corridor, 2 - Industrial


    // Start is called before the first frame update
    void Start()
    {
        output = "";
        text = GetComponent<TextMeshPro>();
        device = "Zion 0.0";
        signal = new Signal();
        text.text = DEFAULT_TEXT;

        pingThread = new Thread(pingCaller.CallPing);
        pingThread.Start();
    }

    private void OnApplicationQuit()
    {
        pingThread.Abort();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CellBeam();

        CheckSignal();
    }


    public void CheckConnectedTower()
    {
        try
        {
            float maxPower = connections.Values.ToList()[0];
            string maxKey = connections.Keys.ToList()[0];
            foreach (var key in connections.Keys.ToList())
            {
                if(connections[key] > maxPower)
                {
                    maxPower = connections[key];
                    maxKey = key;
                }
            }
            sphereObj = GameObject.Find(maxKey);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private void CheckSignal()
    {

        try
        {
            foreach (var key in connections.Keys.ToList())
            {
                tower = GameObject.Find(key);
                connections[key] = GetPower(obj, tower);

                signalLossCount[key] = NewShoot(transform.position, tower);
            }

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public float DistanceÑalculation(GameObject phone, GameObject tower)
    {
        Vector3 vectorDistance = tower.transform.position - phone.transform.position;

        return vectorDistance.magnitude;
    }

    public float GetPower(GameObject phone, GameObject tower)
    {
        Signal newSignal = new Signal();

        float distance = DistanceÑalculation(phone, tower);
        float radius = tower.transform.localScale.x / 2;
        newSignal.ChangeSignal(distance, signalLossCount[tower.name], radius, 
                                tower.GetComponent<CellSphere>().conectedPhones.Count);
      
        return newSignal.power;
    }

    private void CellBeam()
    {
        if (connections.Count != 0)
        {
            if(connections.Count > 1)
            {
                CheckConnectedTower();
            }
            else
            {
                sphereObj = GameObject.Find(connections.Keys.ToList()[0]);
            }


            signal.ChangeSignal(System.Convert.ToInt32(connections[sphereObj.name]));



            try
            {
                bufferPing = pingCaller.ping;
                //Debug.Log(bufferPing);
                text.text = $"{signal.GetNetIndexator()}\n{signal.speed} {signal.signalType}\n_____________\n" +
                    $"{device}\n\nConnected to {sphereObj.name}\n\n" +
                     $"Signal:\n {signal.power} dBm\n\nSignal loss:\n{signalLossCount[sphereObj.name]}" + 
                     $"\nPing:\n {bufferPing}";
                
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }


            double delay = ToCalculateDelay();

            SendDelay(delay);

        }
        else
        {
            text.text = DEFAULT_TEXT;
        }
    }

    public float NewShoot(Vector3 position, GameObject tower)
    {
        float signalLoss = 0;
        RaycastHit[] allHits;
        Vector3 vectorDistance = tower.transform.position - position;

        allHits = Physics.RaycastAll(position, vectorDistance, vectorDistance.magnitude, mask);

        Array.Sort(allHits, CellComm.CompareByDictance);

        collisions[0] = 0; collisions[1] = 0; collisions[2] = 0;

        foreach (RaycastHit hit in allHits)
        {
            signalLoss = SignalLostCalculation(hit, collisions, signalLoss, tower);
        }

        return signalLoss;
    }

    public float SignalLostCalculation(RaycastHit hit, int[] collisions, float signalLoss, GameObject tower)
    {
        switch (hit.collider.tag)
        {
            case "Office":

                collisions[(int)TypeOfBuilding.Office]++;

                if (collisions[(int)TypeOfBuilding.Office] % 2 == 1)
                {
                    lastHits[(int)TypeOfBuilding.Office] = hit;
                }
                else
                {
                    float d = DistanceÑalculation(obj, tower);
                    float r = tower.transform.localScale.x / 2;
                    float f = signal.FrequencyCalculation(d, r);

                    Vector3 vectorDistBetweenHits = hit.point - lastHits[(int)TypeOfBuilding.Office].point;
                    float distBetweenHits = vectorDistBetweenHits.magnitude;
                    Debug.Log(distBetweenHits);

                    signalLoss += (float)(10 * Ratios.OfficeAlfa * Math.Log10(distBetweenHits) +
                                        Ratios.OfficeBeta +
                                        10 * Ratios.OfficeGamma * Math.Log10(f/ Mathf.Pow(10, 9)));
                }

                break;

            case "Corridor":

                collisions[(int)TypeOfBuilding.Corridor]++;

                if (collisions[(int)TypeOfBuilding.Corridor] % 2 == 1)
                {
                    lastHits[(int)TypeOfBuilding.Corridor] = hit;
                }
                else
                {
                    float d = DistanceÑalculation(obj, tower);
                    float r = tower.transform.localScale.x / 2;
                    float f = signal.FrequencyCalculation(d, r);

                    Vector3 vectorDistBetweenHits = hit.point - lastHits[(int)TypeOfBuilding.Corridor].point;
                    float distBetweenHits = vectorDistBetweenHits.magnitude;

                    signalLoss += (float)(10 * Ratios.CorridorAlfa * Math.Log10(hit.distance) +
                                        Ratios.CorridorBeta +
                                        10 * Ratios.CorridorGamma * Math.Log10(f/ Mathf.Pow(10, 9)));
                }

                break;

            case "Industrial":

                collisions[(int)TypeOfBuilding.Industrial]++;

                if (collisions[(int)TypeOfBuilding.Industrial] % 2 == 1)
                {
                    lastHits[(int)TypeOfBuilding.Industrial] = hit;
                }
                else
                {
                    float d = DistanceÑalculation(obj, tower);
                    float r = tower.transform.localScale.x / 2;
                    float f = signal.FrequencyCalculation(d, r);

                    Vector3 vectorDistBetweenHits = hit.point - lastHits[(int)TypeOfBuilding.Industrial].point;
                    float distBetweenHits = vectorDistBetweenHits.magnitude;

                    signalLoss += (float)(10 * Ratios.IndustrialAlfa * Math.Log10(hit.distance) +
                                        Ratios.IndustrialBeta +
                                        10 * Ratios.IndustrialGamma * Math.Log10(f/ Mathf.Pow(10, 9)));
                }

                break;

        }
        return signalLoss;
    }

    public static int CompareByDictance(RaycastHit hit1, RaycastHit hit2)
    {
        return hit1.distance.CompareTo(hit2.distance);
    }

    private double ToCalculateDelay()
    {
        double delay = (sphereObj.transform.localScale.x / 2 * Mathf.Cos(30) * (Mathf.Sqrt(28) + 2)) / (0.69 * 3000000);
        return delay;
    }
    void SendDelay(double delay)
    {
        //print(delay);
    }

}

