using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TriggerType {GROUND, TREEJUMPING}

public class AITrigger : MonoBehaviour
{
    [SerializeField]
    [Tooltip("This specifies which AI function to enable when the player walks through this trigger.")]
    private TriggerType triggerType = TriggerType.GROUND;
    [SerializeField]
    [Tooltip("This will set the wendigo aggression state. (This will be dynamic in the future)")]
    private WendigoState wendigoState = WendigoState.PASSIVE;

    [SerializeField]
    private AIManager aIManager;

    // Start is called before the first frame update
    void Start()
    {
        // Get the AI manager
        aIManager = GameObject.FindObjectOfType<AIManager>();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            Debug.Log("Player hit an AITrigger!", this.gameObject);
            // Need to show the AI
            aIManager.ShowAI();
            
            switch(triggerType)
            {
                case TriggerType.GROUND:
                   aIManager.TriggerAIGroundDashing(wendigoState);
                    break;
                case TriggerType.TREEJUMPING:
                    aIManager.TriggerAITreeJumping();
                    break;
            }
        }
    }
}
