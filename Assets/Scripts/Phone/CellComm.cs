using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class CellComm : MonoBehaviour
{
    public const String DEFAULT_TEXT = "<color=red>no signal...</color>\n\n___________\n\nZion 0.0\n\nNo connection...";
    public GameObject obj;
    public GameObject tower;
    public String device { get; set; }
    public String output { get; set; }
    public TextMeshPro text { get; set; }
    public Signal signal { get; set; }
    public Dictionary<string, float> connections = new Dictionary<string, float>();

    public int collisionsCount { get; set; }
    public GameObject sphereObj;
    public GameObject cellTower;
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



    private void CheckSignal()
    {
        try
        {
            foreach (var key in connections.Keys.ToList())
            {
                tower = GameObject.FindGameObjectWithTag(key);
                Vector3 vectorDistance = tower.transform.position - transform.position;
                float distance = Mathf.Sqrt(vectorDistance.x * vectorDistance.x +
                                            vectorDistance.y * vectorDistance.y + 
                                            vectorDistance.z * vectorDistance.z);
                connections[key] = distance;
                float radius = tower.transform.localScale.x/2;
                signal.Power(distance, collisionsCount, radius, tower.GetComponent<CellSphere>().conectedPhones.Count);
                //signal.Power(distance, collisionsCount, radius, 512);
                //signal.Power(distance, collisionsCount, radius, 256);
                //signal.Power(distance, collisionsCount, radius, 280);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    private void CellBeam()
    {
        sphereObj = GameObject.FindGameObjectWithTag("Sphere_1");
        if (connections.ContainsKey(sphereObj.tag))
        {
            collisionsCount = 0;
            Shoot(transform.position);

            try
            {
                text.text = $"{signal.GetNetIndexator()}\n{signal.power} dBm\n___________\n\n" +
                    $"{device}\n\nSignal:\n {signal.speed} {signal.signalType}\n\n" +
                     $"Distance:\n{connections[sphere.tag]}\n\nCollisions:\n{collisionsCount}";
                
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

        cellTower = GameObject.FindGameObjectWithTag("Tower");
        Vector3 vectorDistance = tower.transform.position - position;

        RaycastHit _hit;

        if (Physics.Raycast(position, vectorDistance, out _hit, 1000f, mask))
        {
            if (_hit.collider.tag == "Building")
            {
                collisionsCount++;
                vectorDistance = tower.transform.position - _hit.point;

                position = _hit.point + (vectorDistance
                            / (Math.Max(vectorDistance.x, Math.Max(vectorDistance.y, vectorDistance.z))));
                Shoot(position);
            }
            else if(_hit.collider.tag == "Tower")
            {
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

