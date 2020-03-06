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
        InvokeRepeating("CheckNearbyGameObjects", 1f, Random.Range(2f, 20f));
    }

    //a way of detecting if a nearbyGameObject is tagged "Tree"
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Tree")
        {
            nearbyGameObjects.Add(other.gameObject);
            //PlayTreeAmbience(other.gameObject);
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

    //a way of playing the FMOD tree ambience event
    void PlayTreeAmbience(GameObject gameObject)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Ambience/sx_wgh_game_amb_tree", gameObject.GetComponent<Transform>().position);
    }

    //a way of checking if the player is still in proximity to relevant gameobjects
    void CheckNearbyGameObjects()
    {
        if(nearbyGameObjects.Count > 0)
        {
            foreach(GameObject g in nearbyGameObjects)
            {
                if(nearbyGameObjects.IndexOf(g) % 2 == 0)
                {
                    PlayTreeAmbience(g);
                }
                
            }
        }
    }
}
