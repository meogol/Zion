using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSphere : MonoBehaviour
{
    public GameObject obj;
    public List<CellComm> conectedPhones;

    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("Sphere_1");
        addConnection(GameObject.Find("PhoneText_1"));
        addConnection(GameObject.Find("PhoneText_2"));
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        addConnection(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        removeConnection(other.gameObject);
    }

    private void addConnection(GameObject phoneObject)
    {
        CellComm phone = phoneObject.GetComponent<CellComm>();
        if (!phone.connections.ContainsKey(obj.tag))
        {
            Vector3 vectorDistance = obj.transform.position - phoneObject.transform.position;
            float distance = Mathf.Sqrt(vectorDistance.x * vectorDistance.x +
                vectorDistance.y * vectorDistance.y + vectorDistance.z * vectorDistance.z);
            phone.connections.Add(obj.tag, distance);
            conectedPhones.Add(phone);
        }
    }

    private void removeConnection(GameObject phoneObject)
    {
        CellComm phone = phoneObject.GetComponent<CellComm>();
        if (phone.connections.ContainsKey(obj.tag))
        {
            phone.connections.Remove(obj.tag);
            conectedPhones.Remove(phone);

        }
    }

}
