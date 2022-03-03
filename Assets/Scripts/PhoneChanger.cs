using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneChanger : MonoBehaviour
{   
    [SerializeField]
    public GameObject playerPhoneHolder;
    [SerializeField]
    public GameObject sourcePhoneHolder;

    private GameObject newPhone;
    private GameObject oldPhone;
    private GameObject transitPhone;
    private string newPhoneName;
    private string oldPhoneName;


    int layerMask = 1 << 7;


    void ChangePhone()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        RaycastHit _hit;

        if (Physics.Raycast(ray, out _hit, Mathf.Infinity, layerMask))
        {
            newPhoneName = _hit.transform.gameObject.name;
            Instantiate(_hit.transform.gameObject, playerPhoneHolder.transform);
            Destroy(_hit.transform.gameObject);


            oldPhone = GameObject.FindGameObjectWithTag("PlayerPhone");
            oldPhoneName = oldPhone.transform.name;
            oldPhone.transform.tag = "Untagged";
            Instantiate(oldPhone, sourcePhoneHolder.transform);


            newPhone = playerPhoneHolder.transform.Find(newPhoneName + "(Clone)").gameObject;
            newPhone.transform.name = newPhoneName;
            newPhone.transform.tag = "PlayerPhone";
            newPhone.transform.localPosition = oldPhone.transform.localPosition;
            newPhone.transform.localRotation = oldPhone.transform.localRotation;
            newPhone.transform.localEulerAngles = oldPhone.transform.localEulerAngles;


            Destroy(oldPhone);


            transitPhone = sourcePhoneHolder.transform.Find(oldPhoneName + "(Clone)").gameObject;
            transitPhone.transform.name = oldPhoneName;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangePhone();
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);
    }
}
