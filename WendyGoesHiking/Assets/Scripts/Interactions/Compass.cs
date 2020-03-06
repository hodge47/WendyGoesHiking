using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The player gameobject.")]
    private GameObject player;
    [SerializeField]
    [Tooltip("The needle of the compass.")]
    private GameObject compassNeedle;
    [SerializeField]
    [Tooltip("The compass needle offset [degrees] in case compass isn't perfectly lined up with player. ")]
    private float compassOffset = 10f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
            compassNeedle.transform.localRotation = Quaternion.Euler(new Vector3(0, player.transform.rotation.eulerAngles.y + compassOffset, 0));
    }
}
