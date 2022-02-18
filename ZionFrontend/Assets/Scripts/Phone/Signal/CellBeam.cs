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
    public int power { get; set; }
    public float distance { get; set; }
    private float speed = 5f;

    [SerializeField]
    private LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        collisionsCount = 0;
        phoneObj = GameObject.FindGameObjectWithTag("Phone");
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        sphereObj = GameObject.FindGameObjectWithTag("Sphere_1");
        if (phone.connections.ContainsKey(sphereObj.tag))
        {
            ToShoot();
            phone.text.text = $"{phone.device}\n\n Signal:\n {phone.signal.speed} {phone.signal.signalType}\n" +
                $"Power:\n { phone.power} { phone.signal.signalType}\n" +
                $"Distance:\n{phone.connections[sphere.tag]}\nCollisions:\n{collisionsCount}";

            transform.position = phone.transform.position;

            Shoot();


            try
            {
                phone.text.text = $"{phone.device}\n\n Signal:\n {phone.signal.speed} {phone.signal.signalType}\n" +
                    $"Power:\n { phone.signal.power} dBm\n" +
                    $"Distance:\n{phone.connections[sphere.tag]}\nCollisions:\n{collisionsCount}";

            }
            catch (Exception e)
            {

            }


            double delay = ToCalculateDelay();

            SendDelay(delay);

        }
        else
        {
            phone.text.text = CellComm.DEFAULT_TEXT;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
    }

    void OnTriggerEnter(Collider other)
    {
        print("trig");
        collisionsCount++;
        print(collisionsCount);
    }

    private void Shoot()
    {
        //print("shoot: " + transform.position);

        Vector3 vectorDistance = sphere.transform.position - phoneObj.transform.position;
        transform.Translate(sphere.transform.position);

        //print("shoot: " + transform.position);



        RaycastHit _hit;
        if(Physics.Raycast(phoneObj.transform.position, vectorDistance, out _hit, 250f, mask))
        {
            Debug.Log(_hit.collider.name);
        }

        //print(_hit);
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
