using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputExample : MonoBehaviour
{

    private PlayerControlActions playerControlActions;

    [SerializeField]
    private float moveX;
    [SerializeField]
    private float moveY;
    [SerializeField]
    private bool reload;
    [SerializeField]
    private bool use;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the player controls with default bindings
        playerControlActions = PlayerControlActions.CreateWithDefaultBindings();
    }

    // Update is called once per frame
    void Update()
    {
        moveX = playerControlActions.MoveX;
        moveY = playerControlActions.MoveY;
        reload = playerControlActions.Reload.IsPressed;
        use = playerControlActions.Use.IsPressed;
    }
}
