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
    [Tooltip("Trigger cooldown time in seconds")]
    [SerializeField]
    private float triggerCooldownTime = 3f;

    private AIManager aIManager;
    private bool firstTrigger = false;
    private float elapsedTimeSinceTrigger = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Get the AI manager
        aIManager = GameObject.FindObjectOfType<AIManager>();
    }

    private void Update()
    {
        // Only trigger the elapsed time counter if first trigger has been hit
        if(firstTrigger == true)
        {
            elapsedTimeSinceTrigger += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            if((firstTrigger == false || elapsedTimeSinceTrigger >= triggerCooldownTime) && aIManager.AiIsAlive)
            {
                //Debug.Log("Player hit an AITrigger!", this.gameObject);
                // Need to show the AI
                aIManager.ShowAI();

                switch (triggerType)
                {
                    case TriggerType.GROUND:
                        aIManager.TriggerAIGroundDashing(wendigoState);
                        break;
                    case TriggerType.TREEJUMPING:
                        aIManager.TriggerAITreeJumping();
                        break;
                }

                if (firstTrigger == false) firstTrigger = true;
                elapsedTimeSinceTrigger = 0f;
            }
        }
    }
}
