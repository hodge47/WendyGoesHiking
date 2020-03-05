using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForceToPlayer : MonoBehaviour
{
    /// <summary>
    /// The force that is applied to the player when they are hit by the wendigo
    /// it should send them in the direction the wendigo is moving
    /// </summary>
    private Vector3 forceToApply;
    [SerializeField]
    private int forceMultiplier;
    //When the player gets hit by the monster they should be flung in the opposite direction of where the wendigo came from.
    //need reference to monster?


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wendigo")
        {
            forceToApply = collision.rigidbody.velocity;
            //GlideController.current.m_rig.velocity = collision.rigidbody.velocity * forceMultiplier;
            GlideController.current.m_rig.AddForceAtPosition((new Vector3(forceToApply.x, 10, forceToApply.z) * forceMultiplier) * Time.deltaTime, this.gameObject.transform.position, ForceMode.VelocityChange);

        }
        
    }

    private void Update()
    {
        
    }
}
