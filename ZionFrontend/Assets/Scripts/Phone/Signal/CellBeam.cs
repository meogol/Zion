using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System;

public class CellBeam : MonoBehaviour
{
    public GameObject obj;
    public GameObject sphereObj;
    public GameObject phoneObj;
    public CellComm phone;
    public CellSphere sphere;
    public int collisionsCount { get; set; }
    public Vector3 towerLocation { get; set; }
    public Vector3 vectorDistance { get; set; }
    public float distance { get; set; }
    private float speed = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        collisionsCount = 0;
        phoneObj = GameObject.FindGameObjectWithTag("Phone");
    }

    // Update is called once per frame
    void Update()
    {

        sphereObj = GameObject.FindGameObjectWithTag("Sphere_1");
        if (phone.connections.ContainsKey(sphereObj.tag))
        {
            ToShoot();
            phone.text.text = $"{phone.device}\n\n Signal:\n {phone.signal.speed} {phone.signal.signalType}\n" +
                $"Distance:\n{phone.connections[sphere.tag]}\nCollisions:\n{collisionsCount}";

            double delay = ToCalculateDelay();

            SendDelay(delay);

        }
        else
        {
            phone.text.text = CellComm.DEFAULT_TEXT;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        print("trig");
        collisionsCount++;
        print(collisionsCount);
    }

    void ToShoot()
    {
        print("shoot");
        collisionsCount = 0;
        Vector3 vectorDistance = sphere.transform.position - transform.position;
        transform.Translate(vectorDistance * speed * Time.deltaTime);
        transform.position = phone.transform.position;
    }

    private double ToCalculateDelay()
    {
        double delay = (sphereObj.transform.localScale.x / 2 * Mathf.Cos(30) * (Mathf.Sqrt(28) + 2)) / (0.69 * 3000000);
        return delay;
    }

    void SendDelay(double delay)
    {
        print(delay);
    }


}
