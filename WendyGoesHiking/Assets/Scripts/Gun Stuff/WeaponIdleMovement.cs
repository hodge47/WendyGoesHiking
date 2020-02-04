using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIdleMovement : MonoBehaviour
{

    private Transform weaponParent;
    private Vector3 weaponParentOrigin;

    // Start is called before the first frame update
    void Start()
    {
        weaponParent = this.gameObject.GetComponent<Transform>();
        weaponParentOrigin = weaponParent.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Headbob(float parameterZ, float xIntensity, float yIntensity)
    {
        weaponParent.localPosition = new Vector3(Mathf.Cos(parameterZ) * xIntensity, Mathf.Sin(parameterZ) * yIntensity, weaponParentOrigin.z);

    }

}
