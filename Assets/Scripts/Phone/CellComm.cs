using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class CellComm : MonoBehaviour
{
    public const String DEFAULT_TEXT = "<color=red>no signal...</color>\n\n_____________\nZion 0.0\n\nNo connection...";
    public GameObject obj;
    public GameObject tower;
    public String device { get; set; }
    public String output { get; set; }
    public TextMeshPro text { get; set; }
    public Signal signal { get; set; }
    public Dictionary<string, float> connections = new Dictionary<string, float>();

    public int collisionsCount { get; set; }
    public GameObject sphereObj;
    public CellSphere sphere;
    [SerializeField]
    private LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {

        collisionsCount = 0;
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
            Debug.Log(e);
        }
    }

    private void CheckSignal()
    {
        try
        {
            foreach (var key in connections.Keys.ToList())
            {
                tower = GameObject.Find(key);
                Vector3 vectorDistance = tower.transform.position - transform.position;
                float distance = Mathf.Sqrt(vectorDistance.x * vectorDistance.x +
                                            vectorDistance.y * vectorDistance.y + 
                                            vectorDistance.z * vectorDistance.z);
                float radius = tower.transform.localScale.x/2;
                signal.power = signal.Power(distance, collisionsCount, radius, tower.GetComponent<CellSphere>().conectedPhones.Count);
                connections[key] = signal.power;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
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
            
            collisionsCount = 0;
            Shoot(transform.position);

            try
            {
                text.text = $"{signal.GetNetIndexator()}\n{signal.power} dBm\n_____________\n" +
                    $"{device}\n\nSignal:\n {signal.speed} {signal.signalType}\n\n" +
                     $"Distance to the {sphereObj.name}:\n{signal.distance}\n\nCollisions:\n{collisionsCount}";
                
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

    private void Shoot(Vector3 position)
    {

        Vector3 vectorDistance = sphereObj.transform.position - position;

        RaycastHit _hit;

        if (Physics.Raycast(position, vectorDistance, out _hit, 1000f, mask))
        {
            if (_hit.collider.tag == "Building")
            {
                collisionsCount++;
                vectorDistance = sphereObj.transform.position - _hit.point;

                position = _hit.point + (vectorDistance
                            / (Math.Max(vectorDistance.x, Math.Max(vectorDistance.y, vectorDistance.z))));
                Shoot(position);
            }
        }
        
        
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

