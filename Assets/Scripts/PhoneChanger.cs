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

    [SerializeField]
    LayerMask layerMask;


    /// <summary>
    /// Method changing cell phone from players hand to the phone player is aiming on
    /// 
    /// oldPhone: the phone that is in the players hands
    /// newPhone: the phone the player is aiming on
    /// transitPhone: is an oldPhone that has been removed from player and added to the spawn point
    /// 
    /// </summary>
    void ChangePhone()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        RaycastHit _hit;

        if (Physics.Raycast(ray, out _hit, Mathf.Infinity, layerMask))
        {
            if (_hit.transform.gameObject.layer == 7)
            {   
                newPhoneName = _hit.transform.gameObject.name;
                Debug.Log(newPhoneName + " newPhoneName");

                Vector3 position = _hit.transform.localPosition;
                Quaternion rotation = _hit.transform.localRotation;
                Vector3 euler = _hit.transform.localEulerAngles;

                Instantiate(_hit.transform.gameObject, playerPhoneHolder.transform);
                Destroy(_hit.transform.gameObject);
            

            
                oldPhone = GameObject.FindGameObjectWithTag("PlayerPhone");
                oldPhoneName = oldPhone.transform.name;
                oldPhone.transform.tag = "Untagged";
                Debug.Log(oldPhoneName + " oldPhoneName");
                Instantiate(oldPhone, sourcePhoneHolder.transform);

                newPhone = playerPhoneHolder.transform.Find(newPhoneName + "(Clone)").gameObject;
                Debug.Log(newPhone.name + " newPhone.name");
                newPhone.transform.name = newPhoneName;
                newPhone.transform.tag = "PlayerPhone";
                Debug.Log(newPhone.name + " newPhone.name");
                newPhone.transform.localPosition = oldPhone.transform.localPosition;
                newPhone.transform.localRotation = oldPhone.transform.localRotation;
                newPhone.transform.localEulerAngles = oldPhone.transform.localEulerAngles;
                newPhone.layer = 7;


                Destroy(oldPhone);
                

                transitPhone = sourcePhoneHolder.transform.Find(oldPhoneName + "(Clone)").gameObject;
                transitPhone.transform.name = oldPhoneName;
                transitPhone.layer = 7;
                transitPhone.transform.localPosition = position;
                transitPhone.transform.localRotation = rotation;
                transitPhone.transform.localEulerAngles = euler;

                Debug.Log(transitPhone.name + " transitphone name" + transitPhone.layer + " transitphone layer");

            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangePhone();
        }
    }
}
