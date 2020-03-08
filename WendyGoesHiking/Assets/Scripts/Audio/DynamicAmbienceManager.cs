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
    //a variable to adjust the maximum number of nearbyGameobjects
    [SerializeField] int maxNearbyGameObjects;
    //a variable to track the current number of nearbyGameObjects
    [SerializeField] int currentNearbyGameObjects;
    //how often to repeat playback of an FMOD event
    [SerializeField] float repeatRate;
    //the minimum possible repeatrate
    [SerializeField] float minRepeatRate;
    //the maximum possible repeatrate
    [SerializeField] float maxRepeatRate;

    private void Start()
    {
        nearbyGameObjects = new List<GameObject>();
        maxNearbyGameObjects = 24;
        minRepeatRate = 3f;
        maxRepeatRate = 15f;
        InvokeRepeating("CheckNearbyGameObjects", 1f, minRepeatRate);
    }

    //a way of detecting if a nearbyGameObject is tagged "Tree"
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Tree")
        {
            //count the number of gameobjects in proximity to the player
            currentNearbyGameObjects = nearbyGameObjects.Count;
            //add the new gameojbect
            nearbyGameObjects.Add(other.gameObject);
            //remove the "oldest" nearby gameobject if currentNearbyGameObjects has too many elements in it
            if(currentNearbyGameObjects >= maxNearbyGameObjects)
            {
                nearbyGameObjects.Remove(nearbyGameObjects[0]);
            }
            
            //PlayTreeAmbience(other.gameObject);
        }
    }

    //a way of detecting if a nearbyGameObject is tagged "Tree" and the player has moved away from it
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Tree")
        {
            nearbyGameObjects.Remove(other.gameObject);
            //count the number of gameobjects in proximity to the player
            currentNearbyGameObjects = nearbyGameObjects.Count;
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
        //count the number of gameobjects in proximity to the player
        currentNearbyGameObjects = nearbyGameObjects.Count;
        //recalculate repeat rate
        CalculateRepeatRate();
        if (currentNearbyGameObjects > 0)
        {
            foreach(GameObject g in nearbyGameObjects)
            {
                if(nearbyGameObjects.IndexOf(g) % 2 == 0)
                {
                    Invoke("PlayTreeAmbience(g)", CalculateRepeatRate());
                    //PlayTreeAmbience(g);
                }
            }
        }
    }
    //a method to determine a float within a random range constrained by the user
    private float CalculateRepeatRate()
    {
        float repeatRateLocal = Random.Range(minRepeatRate, maxRepeatRate);
        return repeatRateLocal;
    }
}
