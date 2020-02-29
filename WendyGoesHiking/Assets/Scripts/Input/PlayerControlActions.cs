using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerControlActions : PlayerActionSet
{
    public PlayerAction MoveRight;
    public PlayerAction MoveLeft;
    public PlayerAction MoveForward;
    public PlayerAction MoveBackward;
    public PlayerAction LookRight;
    public PlayerAction LookLeft;
    public PlayerAction LookUp;
    public PlayerAction LookDown;
    public PlayerAction Jump;
    public PlayerAction Sprint;
    public PlayerAction Shoot;
    public PlayerAction Zoom;
    public PlayerAction Reload;
    public PlayerAction Use;
    public PlayerAction ShotgunSwitch;
    public PlayerAction FlashlightSwitch;
    public PlayerAction CompassSwitch;
    public PlayerOneAxisAction MoveX;
    public PlayerOneAxisAction MoveY;
    public PlayerOneAxisAction LookX;
    public PlayerOneAxisAction LookY;

    public PlayerControlActions()
    {
        MoveRight = CreatePlayerAction("MoveRight");
        MoveLeft = CreatePlayerAction("MoveLeft");
        MoveForward = CreatePlayerAction("MoveForward");
        MoveBackward = CreatePlayerAction("MoveBackward");
        LookRight = CreatePlayerAction("LookRight");
        LookLeft = CreatePlayerAction("LookLeft");
        LookUp = CreatePlayerAction("LookUp");
        LookDown = CreatePlayerAction("LookDown");
        Jump = CreatePlayerAction("Jump");
        Sprint = CreatePlayerAction("Sprint");
        Shoot = CreatePlayerAction("Shoot");
        Zoom = CreatePlayerAction("Zoom");
        Reload = CreatePlayerAction("Reload");
        Use = CreatePlayerAction("Use");
        ShotgunSwitch = CreatePlayerAction("ShotgunSwitch");
        FlashlightSwitch = CreatePlayerAction("FlashlightSwitch");
        CompassSwitch = CreatePlayerAction("CompassSwitch");
        MoveX = CreateOneAxisPlayerAction(MoveLeft, MoveRight);
        MoveY = CreateOneAxisPlayerAction(MoveBackward, MoveForward);
        LookX = CreateOneAxisPlayerAction(LookLeft, LookRight);
        LookY = CreateOneAxisPlayerAction(LookDown, LookUp);
    }

    public static PlayerControlActions CreateWithDefaultBindings()
    {
        var playerControlActions = new PlayerControlActions();

        // Keyboard and mouse inputs
        playerControlActions.MoveRight.AddDefaultBinding(Key.D);
        playerControlActions.MoveLeft.AddDefaultBinding(Key.A);
        playerControlActions.MoveForward.AddDefaultBinding(Key.W);
        playerControlActions.MoveBackward.AddDefaultBinding(Key.S);
        playerControlActions.LookRight.AddDefaultBinding(Mouse.PositiveX);
        playerControlActions.LookLeft.AddDefaultBinding(Mouse.NegativeX);
        playerControlActions.LookUp.AddDefaultBinding(Mouse.PositiveY);
        playerControlActions.LookDown.AddDefaultBinding(Mouse.NegativeY);
        playerControlActions.Jump.AddDefaultBinding(Key.Space);
        playerControlActions.Sprint.AddDefaultBinding(Key.LeftShift);
        playerControlActions.Shoot.AddDefaultBinding(Mouse.LeftButton);
        playerControlActions.Zoom.AddDefaultBinding(Mouse.RightButton);
        playerControlActions.Reload.AddDefaultBinding(Key.R);
        playerControlActions.Use.AddDefaultBinding(Key.E);
        playerControlActions.ShotgunSwitch.AddDefaultBinding(Key.Key1);
        playerControlActions.FlashlightSwitch.AddDefaultBinding(Key.Key2);
        playerControlActions.CompassSwitch.AddDefaultBinding(Key.Key3);
        // Controller inputs
        playerControlActions.MoveRight.AddDefaultBinding(InputControlType.LeftStickRight);
        playerControlActions.MoveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
        playerControlActions.MoveForward.AddDefaultBinding(InputControlType.LeftStickUp);
        playerControlActions.MoveBackward.AddDefaultBinding(InputControlType.LeftStickDown);
        playerControlActions.LookRight.AddDefaultBinding(InputControlType.RightStickRight);
        playerControlActions.LookLeft.AddDefaultBinding(InputControlType.RightStickLeft);
        playerControlActions.LookUp.AddDefaultBinding(InputControlType.RightStickUp);
        playerControlActions.LookDown.AddDefaultBinding(InputControlType.RightStickDown);
        playerControlActions.Jump.AddDefaultBinding(InputControlType.Button2);
        playerControlActions.Sprint.AddDefaultBinding(InputControlType.LeftStickButton);
        playerControlActions.Shoot.AddDefaultBinding(InputControlType.RightTrigger);
        playerControlActions.Zoom.AddDefaultBinding(InputControlType.LeftTrigger);
        playerControlActions.Reload.AddDefaultBinding(InputControlType.Button3);
        playerControlActions.Use.AddDefaultBinding(InputControlType.Button1);
        playerControlActions.ShotgunSwitch.AddDefaultBinding(InputControlType.DPadUp);
        playerControlActions.FlashlightSwitch.AddDefaultBinding(InputControlType.DPadRight);
        playerControlActions.CompassSwitch.AddDefaultBinding(InputControlType.DPadDown);

        playerControlActions.ListenOptions.IncludeUnknownControllers = true;
        playerControlActions.ListenOptions.MaxAllowedBindings = 2;

        playerControlActions.ListenOptions.AllowDuplicateBindingsPerSet = false;
        playerControlActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;
        playerControlActions.ListenOptions.IncludeMouseButtons = true;

        playerControlActions.ListenOptions.OnBindingFound = ( action, binding ) => {
            if (binding == new KeyBindingSource( Key.Escape ))
            {
                action.StopListeningForBinding();
                return false;
            }
            return true;
        };

        playerControlActions.ListenOptions.OnBindingAdded += ( action, binding ) => {
            Debug.Log( "Binding added... " + binding.DeviceName + ": " + binding.Name );
        };

        playerControlActions.ListenOptions.OnBindingRejected += ( action, binding, reason ) => {
            Debug.Log( "Binding rejected... " + reason );
        };

        return playerControlActions;
    }
}
