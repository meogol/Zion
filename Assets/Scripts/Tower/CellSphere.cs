using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSphere : MonoBehaviour
{
    public const int DEFAULT_COUNT_OF_USRES = 666;
    public int countOfUsers;
    public GameObject obj;
    public GameObject phoneObj;
    public CellComm phone;

    // Start is called before the first frame update
    void Start()
    {
        phoneObj = GameObject.FindGameObjectWithTag("Phone");


    }

    // Update is called once per frame
    void Update()
    {
        countOfUsers = DEFAULT_COUNT_OF_USRES;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!phone.connections.ContainsKey(obj.tag))
        {
            Vector3 vectorDistance = obj.transform.position - phoneObj.transform.position;
            float distance = Mathf.Sqrt(vectorDistance.x * vectorDistance.x + 
                vectorDistance.y * vectorDistance.y + vectorDistance.z * vectorDistance.z);
            phone.connections.Add(obj.tag, distance);
            countOfUsers++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (phone.connections.ContainsKey(obj.tag))
        {
            phone.connections.Remove(obj.tag);
            countOfUsers--;
        }
    }

}
