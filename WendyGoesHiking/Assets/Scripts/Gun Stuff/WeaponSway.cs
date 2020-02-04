using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float intensity;
    public float adjustmentSpeed;

    private Quaternion originRotation;

    private void Start()
    {
        originRotation = transform.localRotation;
    }

    private void Update()
    {
        UpdateSway();    
    }

    private void UpdateSway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //calculate target rotation
        Quaternion adjustmentAngleX = Quaternion.AngleAxis(-intensity* mouseX, Vector3.up);
        Quaternion adjustmentAngleY = Quaternion.AngleAxis(intensity* mouseY, Vector3.right);
        Quaternion targetRotation = originRotation * adjustmentAngleX * adjustmentAngleY;

        //rotate towards target rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * adjustmentSpeed);

    }


}

