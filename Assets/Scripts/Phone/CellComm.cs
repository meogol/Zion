using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class CellComm : MonoBehaviour
{
    public const String DEFAULT_TEXT = "<color=red>no signal...</color>\n0 Mbps\n_____________\nZion 0.0\n\nNo connection...";
    public GameObject obj;
    public GameObject tower;
    public String device { get; set; }
    public String output { get; set; }
    public TextMeshPro text { get; set; }
    public Dictionary<string, float> connections = new Dictionary<string, float>();

    public Dictionary<string, int> collisionsCount = new Dictionary<string, int>();
    public GameObject sphereObj;
    public CellSphere sphere;
    public Signal signal { get; set; }
    [SerializeField]
    private LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        output = "";
        text = GetComponent<TextMeshPro>();
        device = "Zion 0.0";
        signal = new Signal();
        text.text = DEFAULT_TEXT;
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

                collisionsCount[key] = Shoot(transform.position, tower);
            }

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public float GetPower(GameObject phone, GameObject tower)
    {
        Signal newSignal = new Signal();
        Vector3 vectorDistance = tower.transform.position - phone.transform.position;
        float distance = Mathf.Sqrt(vectorDistance.x * vectorDistance.x +
                                    vectorDistance.y * vectorDistance.y +
                                    vectorDistance.z * vectorDistance.z);
        float radius = tower.transform.localScale.x / 2;
        newSignal.ChangeSignal(distance, collisionsCount[tower.name], radius, 
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
                text.text = $"{signal.GetNetIndexator()}\n{signal.speed} {signal.signalType}\n_____________\n" +
                    $"{device}\n\nConnected to {sphereObj.name}\n\n" +
                     $"Signal:\n {signal.power} dBm\n\nCollisions:\n{collisionsCount[sphereObj.name]}";
                
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

    public int Shoot(Vector3 position, GameObject tower)
    {
        int collisions = 0;
        Vector3 vectorDistance = tower.transform.position - position;

        RaycastHit _hit;

        if (Physics.Raycast(position, vectorDistance, out _hit, 1000f, mask))
        {
            if (_hit.collider.tag == "Building")
            {
                collisions++;
                vectorDistance = tower.transform.position - _hit.point;

                position = _hit.point + (vectorDistance
                            / (Math.Max(vectorDistance.x, Math.Max(vectorDistance.y, vectorDistance.z))));
                collisions += Shoot(position, tower);
            }
        }


        return collisions;
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

