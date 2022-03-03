using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDirection : MonoBehaviour
{
    public LayerMask layerMask;
    public GameObject textholder;


    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            textholder.SetActive(true);
        }
        else
        {
            textholder.SetActive(false);
        }
    }
}
