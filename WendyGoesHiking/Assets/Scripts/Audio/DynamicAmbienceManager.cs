using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicAmbienceManager : MonoBehaviour
{
    ///jpost Audio
    ///A class inspired by the idea of "frustrum culling" meant to dynamically instantiate and manage the lifecycle of FMOD ambience events
    ///based on proximity to the player character
    ///

    //a list of gameObjects
    [SerializeField] List<GameObject> nearbyGameObjects;

    private void Start()
    {
        nearbyGameObjects = new List<GameObject>();
    }

    //a way of detecting if a nearbyGameObject is tagged "Tree"
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Tree")
        {
            nearbyGameObjects.Add(other.gameObject);
        }
    }
    //a way of detecting if a nearbyGameObject is tagged "Tree" and the player has moved away from it
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Tree")
        {
            nearbyGameObjects.Remove(other.gameObject);
        }
    }
}
