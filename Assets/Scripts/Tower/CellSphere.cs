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


        addConnection(GameObject.Find("PhoneText"));
        /*
         * Добавление телефонов
         *
        addConnection(GameObject.Find("PhoneText_2"));
        */
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PhoneText")
        {
            addConnection(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PhoneText")
        {
            removeConnection(other.gameObject);
        }
    }

    private void addConnection(GameObject phoneObject)
    {   
        CellComm phone = phoneObject.GetComponent<CellComm>();
        if (!phone.connections.ContainsKey(obj.name))
        {
            Vector3 vectorDistance = obj.transform.position - phoneObject.transform.position;
            float distance = Mathf.Sqrt(vectorDistance.x * vectorDistance.x +
                vectorDistance.y * vectorDistance.y + vectorDistance.z * vectorDistance.z);

            phone.signalLossCount.Add(obj.name, phone.NewShoot(phoneObject.transform.position, obj));
            float power = phone.GetPower(phoneObject, obj);
            phone.connections.Add(obj.name, power);
            conectedPhones.Add(phone);
        }
    }

    private void removeConnection(GameObject phoneObject)
    {
        CellComm phone = phoneObject.GetComponent<CellComm>();
        if (phone.connections.ContainsKey(obj.name))
        {
            phone.connections.Remove(obj.name);
            phone.signalLossCount.Remove(obj.name);
            conectedPhones.Remove(phone);

        }
    }

}
