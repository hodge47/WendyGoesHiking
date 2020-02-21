using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CalculateDistance : MonoBehaviour
{
    public GameObject PosA;
    public GameObject PosB;
    [Button(ButtonSizes.Small)]
    private void CalculateDist()
    {
        Debug.Log(Vector3.Distance(PosA.transform.position, PosB.transform.position));
    }
}
